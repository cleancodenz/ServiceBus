

using System;
using System.IO;
using System.IO.Compression;
using System.ServiceModel.Channels;

namespace MyBehaviors
{
    //The service framework uses this property to obtain an encoder from this encoder factory
    internal class GZipMessageEncoder : MessageEncoder
    {
        static string GZipContentType = "application/x-gzip";

        //This implementation wraps an inner encoder that actually converts a WCF Message
        //into textual XML, binary XML or some other format. This implementation then compresses the results.
        //The opposite happens when reading messages.
        //This member stores this inner encoder.
        MessageEncoder innerEncoder;

        //We require an inner encoder to be supplied (see comment above)
        internal GZipMessageEncoder(MessageEncoder messageEncoder)
            : base()
        {
            if (messageEncoder == null)
                throw new ArgumentNullException("messageEncoder", "A valid message encoder must be passed to the GZipEncoder");
            innerEncoder = messageEncoder;
        }

        public override string ContentType
        {
            get { return GZipContentType; }
        }

        public override string MediaType
        {
            get { return GZipContentType; }
        }

        //SOAP version to use - we delegate to the inner encoder for this
        public override MessageVersion MessageVersion
        {
            get { return innerEncoder.MessageVersion; }
        }

        //One of the two main entry points into the encoder. Called by WCF to decode a buffered byte array into a Message.
        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            //Decompress the buffer
            ArraySegment<byte> decompressedBuffer = DecompressBuffer(buffer, bufferManager);
            //Use the inner encoder to decode the decompressed buffer
            Message returnMessage = innerEncoder.ReadMessage(decompressedBuffer, bufferManager);
            
            // for debugging
            var content = returnMessage.ToString();

            returnMessage.Properties.Encoder = this;
            return returnMessage;

        }

        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
        {
            //Pass false for the "leaveOpen" parameter to the GZipStream constructor.
            //This will ensure that the inner stream gets closed when the message gets closed, which
            //will ensure that resources are available for reuse/release.
            GZipStream gzStream = new GZipStream(stream, CompressionMode.Decompress, false);
            var message = innerEncoder.ReadMessage(gzStream, maxSizeOfHeaders);
            return message;
        }

        //One of the two main entry points into the encoder. Called by WCF to encode a Message into a buffered byte array.
        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            //The message is ServiceModel.Security.SecurityAppliedMessage

            //Use the inner encoder to encode a Message into a buffered byte array
            ArraySegment<byte> buffer = innerEncoder.WriteMessage(message, maxMessageSize, bufferManager, 0);
            //Compress the resulting byte array
            return CompressBuffer(buffer, bufferManager, messageOffset);
        }

        public override void WriteMessage(Message message, Stream stream)
        {
            using (GZipStream gzStream = new GZipStream(stream, CompressionMode.Compress, true))
            {
                innerEncoder.WriteMessage(message, gzStream);
            }

            // innerEncoder.WriteMessage(message, gzStream) depends on that it can flush data by flushing 
            // the stream passed in, but the implementation of GZipStream.Flush will not flush underlying
            // stream, so we need to flush here.
            stream.Flush();
        }

        //Helper method to compress an array of bytes
        static ArraySegment<byte> CompressBuffer(ArraySegment<byte> buffer, BufferManager bufferManager, int messageOffset)
        {
            MemoryStream memoryStream = new MemoryStream();

            using (GZipStream gzStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gzStream.Write(buffer.Array, buffer.Offset, buffer.Count);
            }

            byte[] compressedBytes = memoryStream.ToArray();
            int totalLength = messageOffset + compressedBytes.Length;
            byte[] bufferedBytes = bufferManager.TakeBuffer(totalLength);

            Array.Copy(compressedBytes, 0, bufferedBytes, messageOffset, compressedBytes.Length);

            bufferManager.ReturnBuffer(buffer.Array);
            ArraySegment<byte> byteArray = new ArraySegment<byte>(bufferedBytes, messageOffset, bufferedBytes.Length - messageOffset);

            return byteArray;
        }

        //Helper method to decompress an array of bytes
        static ArraySegment<byte> DecompressBuffer(ArraySegment<byte> buffer, BufferManager bufferManager)
        {
            MemoryStream memoryStream = new MemoryStream(buffer.Array, buffer.Offset, buffer.Count);
            MemoryStream decompressedStream = new MemoryStream();
            int totalRead = 0;
            int blockSize = 1024;
            byte[] tempBuffer = bufferManager.TakeBuffer(blockSize);
            using (GZipStream gzStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                while (true)
                {
                    int bytesRead = gzStream.Read(tempBuffer, 0, blockSize);
                    if (bytesRead == 0)
                        break;
                    decompressedStream.Write(tempBuffer, 0, bytesRead);
                    totalRead += bytesRead;
                }
            }
            bufferManager.ReturnBuffer(tempBuffer);

            byte[] decompressedBytes = decompressedStream.ToArray();
            byte[] bufferManagerBuffer = bufferManager.TakeBuffer(decompressedBytes.Length + buffer.Offset);
            Array.Copy(buffer.Array, 0, bufferManagerBuffer, 0, buffer.Offset);
            Array.Copy(decompressedBytes, 0, bufferManagerBuffer, buffer.Offset, decompressedBytes.Length);

            ArraySegment<byte> byteArray = new ArraySegment<byte>(bufferManagerBuffer, buffer.Offset, decompressedBytes.Length);
            bufferManager.ReturnBuffer(buffer.Array);

            return byteArray;
        }
    }
}

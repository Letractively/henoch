using System.IO;
using System.Net;
using System.Text;

namespace AsyncHandlers
{
    public class RequestState
    {
        // This class stores the State of the request.
        public static readonly int BUFFER_SIZE = 1024;
        public StringBuilder requestData;
        public byte[] BufferRead;
        public HttpWebRequest request;
        public HttpWebResponse response;
        public Stream streamResponse;
        public RequestState()
        {
            BufferRead = new byte[BUFFER_SIZE];
            requestData = new StringBuilder("");
            request = null;
            streamResponse = null;
        }
    }
}
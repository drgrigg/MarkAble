using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace MarkAble2
{
    public delegate void GotVersionHandler();

    public class RequestState
    {
        // This class stores the state of the request.
        const int BUFFER_SIZE = 1024;
        public StringBuilder requestData;
        public byte[] bufferRead;
        public WebRequest request;
        public WebResponse response;
        public Stream responseStream;
        public RequestState()
        {
            bufferRead = new byte[BUFFER_SIZE];
            requestData = new StringBuilder("");
            request = null;
            responseStream = null;
        }
    }


    public class VersionCheck:IComparable<VersionCheck>
    {
        public event GotVersionHandler GotVersion;

        public static ManualResetEvent allDone = new ManualResetEvent(false);


        public int Major = 0;
        public int Minor = 0;
        public int Revision = 0;
        public bool OK = false;


        const int DefaultTimeout = 3 * 1000; // 3 secs timeout

        // Abort the request if the timer fires.
        private static void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null)
                {
                    request.Abort();
                }
            }
        }



        public VersionCheck()
        {
            
        }

        public VersionCheck(string versionstring)
        {
            SetVersionFromString(versionstring);
        }

        private void SetVersionFromString(string versionstring)
        {
            string[] parts = versionstring.Split(new char[] {'.','(',')'}, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 3)
            {
                try
                {
                    Major = Convert.ToInt32(parts[0].Trim());
                    Minor = Convert.ToInt32(parts[1].Trim());
                    Revision = Convert.ToInt32(parts[2].Trim());
                    OK = true;
                }
                catch
                {
                    OK = false;
                }
            }
            else
            {
                OK = false;
            }          
        }

        protected int CompareValue
        {
            get { return Major*1000000 + Minor*1000 + Revision; }
        }

        public void RequestCurrentVersion(string urlstr)
        {          
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(urlstr);

                var requestState = new RequestState();
                requestState.request = webRequest;

                webRequest.BeginGetResponse(new AsyncCallback(RespCallback), requestState);

                // Start the asynchronous request.
                IAsyncResult result =
                  (IAsyncResult)webRequest.BeginGetResponse(new AsyncCallback(RespCallback), requestState);

                // this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
                ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), webRequest, DefaultTimeout, true);


                allDone.WaitOne(1000,false);
                requestState.response.Close();
            }
            catch
            {
                //do nowt
            }
        }

        private void RespCallback(IAsyncResult asynchronousResult)
        {
            RequestState myRequestState = null;
            Stream dataStream = null;
            StreamReader reader = null;

            try
            {
                myRequestState = (RequestState)asynchronousResult.AsyncState;
                var myWebRequest1 = myRequestState.request;
                // End the Asynchronous response.
                myRequestState.response = myWebRequest1.EndGetResponse(asynchronousResult);
                // Read the response into a 'Stream' object.
                dataStream = myRequestState.response.GetResponseStream();
                reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                SetVersionFromString(responseFromServer);

                if (this.OK)
                    GotVersion();
            }
            catch 
            {
                //do nowt    
            }
            finally
            {
                if (reader != null) reader.Close();
                if (dataStream != null) dataStream.Close();
                if (myRequestState != null)
                {
                    try
                    {
                        myRequestState.response.Close();
                    }
                    catch 
                    {
                        //do nowt
                    }

                }
            }

        }

        public override string ToString()
        {
            return Major.ToString() + "." + Minor.ToString() + "." + Revision.ToString();
        }

        #region IComparable<VersionCheck> Members

        public int CompareTo(VersionCheck other)
        {
            if ((!OK) && (!other.OK))
                return 0;

            if (!OK)
                return -1;

            if (!other.OK)
                return -1;

            return this.CompareValue.CompareTo(other.CompareValue);
        }

        #endregion


    }
}

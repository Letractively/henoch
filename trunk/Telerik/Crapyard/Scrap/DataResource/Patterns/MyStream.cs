using System;
using System.Collections.ObjectModel;

namespace DataResource.Patterns
{
    /// <summary>
    /// Represents a stream or a collection
    /// </summary>
    /// <typeparam name="TDataConsumer"></typeparam>
    public class MyStream<TDataConsumer> : IMyStream
        where TDataConsumer : IDataConsumer, new( ) 
    {
        private readonly TDataConsumer MyDataConsumer;
        private bool Disposed;
        public string FileName { get; set; }

        public MyStream( ) 
        {
            MyDataConsumer = new TDataConsumer( );
            ///Code coverage tool cannot verify
        }
        public MyStream(string fileName)
        {
            MyDataConsumer = new TDataConsumer();
            MyDataConsumer.FileName = fileName;
            ///Code coverage tool cannot verify
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            // Implement IDisposable.
            // Do not make this method virtual.
            // A derived class should not be able to override this method.
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);

        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be Disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be Disposed.
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!Disposed)
            {
                
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    MyDataConsumer.Dispose();
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                // Note disposing has been done.
                Disposed = true;

            }
        }


        #endregion

        #region Implementation of IMyStream

        public string DeploymentDir { get; set; }

        public bool Open(string fileNaam)
        {
            return MyDataConsumer.Open(fileNaam);
        }

        public string ReadLine()
        {
            return MyDataConsumer.ReadLine();
        }

        public bool Close()
        {
            return MyDataConsumer.Close();
        }

        #endregion
    }
}
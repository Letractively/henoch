using System;

namespace MyDataConsumer
{
    public interface IDirectory
    {
        /// <summary>
        /// This is the dir containing the subdirs for output.
        /// </summary>
        string ResultDir { get; set; }

        /// <summary>
        /// This is the subdir where the files are saved, relative to Resultdir.
        /// </summary>
        string OutputDir { get; set; }

        /// <summary>
        /// This is the root where the output files are deployed.
        /// </summary>
        string DeploymentDirectory { get; set; }

        /// <summary>
        /// This is the root dir for the adapted version of the prototype.
        /// </summary>
        string RootDirectory { get; set; }

        string ImplicDirectory { get; set; }
        string FaalkansDirectory { get; set; }
        string TnoDirectory { get; set; }
        string ToetsPeilenDirectory { get; set; }
    }

    public class MyDirectory<TDataConsumer> : IDirectory
        where TDataConsumer : IDataConsumer, new( ) 
    {
        private readonly TDataConsumer MyDataConsumer;
        public MyDirectory()
        {
            MyDataConsumer = new TDataConsumer();            
        }
        #region Implementation of IDirectory

        public string ResultDir
        {
            get { return MyDataConsumer.ResultDir; }
            set { MyDataConsumer.ResultDir = value;}
        }

        public string OutputDir
        {
            get { return MyDataConsumer.OutputDir; }
            set { MyDataConsumer.OutputDir = value; }
        }

        public string DeploymentDirectory
        {
            get { return MyDataConsumer.DeploymentDirectory; }
            set { MyDataConsumer.DeploymentDirectory = value; }
        }

        public string RootDirectory
        {
            get { return MyDataConsumer.RootDirectory; }
            set { MyDataConsumer.RootDirectory = value; }
        }
        public string ImplicDirectory
        {
            get { return MyDataConsumer.ImplicDirectory; }
            set { MyDataConsumer.ImplicDirectory = value; }
        }

        public string FaalkansDirectory
        {
            get { return MyDataConsumer.FaalkansDirectory; }
            set { MyDataConsumer.FaalkansDirectory = value; }
        }

        public string TnoDirectory
        {
            get { return MyDataConsumer.TnoDirectory; }
            set { MyDataConsumer.TnoDirectory = value; }
        }

        public string ToetsPeilenDirectory
        {
            get { return MyDataConsumer.ToetsPeilenDirectory; }
            set { MyDataConsumer.ToetsPeilenDirectory = value; }
        }

        #endregion
    }
}
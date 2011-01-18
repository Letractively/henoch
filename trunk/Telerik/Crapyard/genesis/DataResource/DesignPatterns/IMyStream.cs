using System;
using System.Collections.ObjectModel;

namespace MyDataConsumer
{
    public interface IMyStream:IDisposable
    {
        string FileName { get; set; }
        bool Open(string fileNaam);
        string ReadLine();
        new void Dispose();
        bool Close();
    }
}
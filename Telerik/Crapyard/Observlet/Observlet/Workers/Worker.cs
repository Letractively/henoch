using System;
using System.Globalization;
using System.Threading;
using ApplicationTypes.DesignPatterns;

namespace Observlet.Workers
{
    public class Worker : LoggingBase
    {
        /// <summary>
        /// Dummy lightweight workload.
        /// </summary>
        public void DoDummyWork()
        {
            try
            {

                for (int i = 0; i < 30; i++)
                {

                    double result = (i);

                    m_Message = "Message\t" + DateTime.Now + "\t:" + string.Format(CultureInfo.InvariantCulture, "precision = {0}\n",
                            Convert.ToDouble(result).ToString("F16", CultureInfo.InvariantCulture));
                    NotifyObserver(m_Message);

                    if (Cancel)
                    {
                        m_Message = "Geannuleerd.";
                        break;
                    }
                    m_Message = "Gereed.";
                    Thread.Sleep(200);
                }


            }
            catch (ThreadInterruptedException)
            {
                m_Message = "interrupted..";
                Console.WriteLine("~~~~ thread2 interrupted...");
            }
            catch (Exception)
            {
                m_Message = "interrupted...";
                Console.WriteLine("~~~~ thread2 error...");
            }
            finally
            {
                Console.WriteLine("~~~~ thread2 initData2 ends ...~~~~");

            }
        }
    }





}

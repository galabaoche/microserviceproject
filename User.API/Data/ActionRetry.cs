using System;
using System.Threading;

namespace User.API.Data
{
    public class ActionRetry
    {
        public static bool RetryDoWork<T1, T2>(Action<T1, T2> action, T1 t1, T2 t2, int retryCount = 3)
        {
            return RetryDoWork(action, t1, t2, retryCount,0);
        }
        public static bool RetryDoWork<T1, T2>(Action<T1, T2> action, T1 t1, T2 t2, int retryCount, int currentRetryCount = 0)
        {

            try
            {
                action(t1, t2);
                currentRetryCount++;
            }
            catch (Exception ex)
            {
                if (currentRetryCount <= retryCount)
                {
                    Thread.Sleep(1000);
                    action(t1, t2);
                }
                else
                {
                    return false;
                    throw new Exception(ex.Message);
                }

            }
            return true;
        }
    }
}
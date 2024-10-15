using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Core.Lib
{
    public class SerialCommunication : INotifyObject
    {
        private SerialPort _port = null;
        public SerialPort Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }

        private int _expected = 0;
        public int Expected
        {
            get { return _expected; }
            set => SetProperty(ref _expected, value);
        }

        public int TimeoutSeconds = 120;//secunde

        private byte[] received = new byte[0];
        private bool isReceiving = false;
        CancellationTokenSource CancellationTokenSource;
        CancellationTokenSource AwaitCancellationTokenSource;

        public SerialCommunication(string PortName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            Port = new SerialPort(PortName, baudRate, parity, dataBits, StopBits.One);
        }

        public SerialCommunication(SerialPort port)
        {
            Port = port;
        }

        public bool EnsureConnection()
        {
            if (!Port.IsOpen)
            {
                try
                {
                    Port.RtsEnable = true;
                    Port.DtrEnable = true;
                    Port.Open();
                }
                catch (Exception ex)
                {
                }
            }
            if (Port.IsOpen)
            {
                Port.DataReceived += Port_DataReceived;
            }
            return Port.IsOpen;
        }

        public void CloseConnection()
        {
            try
            {
                Port.DataReceived -= Port_DataReceived;
                Port.DiscardInBuffer();
                Port.DiscardOutBuffer();
                Port.Close();
                Port.Dispose();
            }
            catch (Exception ex) { }
        }

        public void Cancel()
        {
            try
            {
                CancellationTokenSource.Cancel();
                AwaitCancellationTokenSource.Cancel();
            }
            catch (Exception ex)
            {

            }
        }

        public Task<byte[]> AwaitResult(byte stop, int pading)
        {
            isReceiving = false;
            received = new byte[0];
            StartTimeoutTimer();
            AwaitCancellationTokenSource = new CancellationTokenSource();
            CancellationToken awaitCancellationToken = AwaitCancellationTokenSource.Token;
            int len = 0;
            int notreceiveng = 0;
            bool canBreak = false;
            return Task.Run(() =>
            {
                while (true)
                {
                    Log.WriteLine("not receving" + notreceiveng);

                    int offset = received.Length - pading - 1;
                    int offset2 = received.Length - pading;

                    if ((offset > 0 && received.Length > offset && received[offset] == stop) || (offset2 > 0 && received.Length > offset2 && received[offset2] == stop))
                    {
                        canBreak = true;
                        // break;
                    }
                    if (awaitCancellationToken.IsCancellationRequested)
                    {
                        StopTimeoutTimer();
                        break;
                    }

                    if (len != received.Length)
                    {
                        len = received.Length;
                    }
                    else
                    {
                        if (isReceiving)
                        {
                            notreceiveng++;
                        }
                    }
                    Thread.Sleep(100);
                    if (notreceiveng > 10)
                    {
                        if (received.Contains(stop) || canBreak)
                        {
                            break;
                        }
                    }
                }
                StopTimeoutTimer();
                return received;
            });
        }
        public Task<byte[]> GetBytes(byte[] bytesToSend, int expected)
        {
            CancellationTokenSource = new CancellationTokenSource();
            CancellationToken CancellationToken = CancellationTokenSource.Token;
            Expected = expected;
            received = new byte[0];

            return Task.Run(() =>
            {
                try
                {
                    Port.Write(bytesToSend, 0, bytesToSend.Length);
                    StartTimeoutTimer();
                }
                catch (Exception)
                {
                    StopTimeoutTimer();
                    return received;
                }
                while (received.Length < expected)
                {
                    if (CancellationToken.IsCancellationRequested)
                    {
                        StopTimeoutTimer();
                        break;
                    }
                    Thread.Sleep(100);
                }
                Log.WriteLine("returning");
                StopTimeoutTimer();
                return received;
            });
        }

        public Task<string> GetString(byte[] bytesToSend, int expected)
        {
            return Task.Run(async () =>
            {
                byte[] bytes = await GetBytes(bytesToSend, expected);
                return System.Text.Encoding.ASCII.GetString(bytes);
            });
        }
        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            isReceiving = true;
            SerialPort port = (SerialPort)sender;
            byte[] data = new byte[port.BytesToRead];
            for (int offset = 0; offset < data.Length;)
            {
                int n = port.Read(data, offset, data.Length - offset);
                offset += n;
            }
            received = BytesMerge(received, data);
            Log.WriteLine("Received = " + BitConverter.ToString(received));
        }

        System.Timers.Timer timoutTimer;
        private void StartTimeoutTimer()
        {
            Log.WriteLine("Timer start");
            timoutTimer = new System.Timers.Timer();
            timoutTimer.Interval = TimeoutSeconds * 1000;
            timoutTimer.Elapsed += (s, e) =>
            {
                Log.WriteLine("timer elapsed");
                timoutTimer.Stop();
                timoutTimer.Dispose();
                try
                {
                    CancellationTokenSource.Cancel();
                }
                catch (Exception ex) { }
                try
                {
                    AwaitCancellationTokenSource.Cancel();
                }
                catch (Exception ex) { }
            };
            timoutTimer.Start();
        }
        public void StopTimeoutTimer()
        {
            if (timoutTimer == null)
            {
                return;
            }
            timoutTimer.Stop();
            timoutTimer.Dispose();
        }

        public static byte[] BytesMerge(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
    }
}

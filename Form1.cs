using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Speech.Synthesis;


namespace CpuMonitor_GUI
{
    public partial class Form1 : Form
    {
        static System.Threading.Thread t;
        int cval;
        int rval;
        String mesg = "";

        static System.Diagnostics.Process process = new System.Diagnostics.Process();
        static System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        static SpeechSynthesizer synth = new SpeechSynthesizer();
        static PerformanceCounter cpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
        static PerformanceCounter memCount = new PerformanceCounter("Memory", "% Committed Bytes in Use");
        static Stopwatch stopWatch = new Stopwatch();
        static TimeSpan ts;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            t = new System.Threading.Thread(monitor_system);
            t.Start();
            cpu_text.Text = "Calculating...";
            ram_text.Text = "Calculating...";
        }

        public void monitor_system()
        {
            mesg = "HI Sejong Students";
            mesg_text.Invoke(new MethodInvoker(setmesg));
            aSpeak(mesg);

            mesg = "CPU Monitor is monitoring your CPU and RAM usage now !!!";
            mesg_text.Invoke(new MethodInvoker(setmesg));
            aSpeak(mesg);

            mesg = "In case of extremely high usage appropriate messages will be sent and necessary measures will be taken !!!";
            mesg_text.Invoke(new MethodInvoker(setmesg));
            aSpeak(mesg);

            cval = (int)cpuCount.NextValue();
            cpu_text.Invoke(new MethodInvoker(setcpu));

            rval = (int)memCount.NextValue();
            ram_text.Invoke(new MethodInvoker(setram));

            String cpuMesg = String.Format("Current CPU Usage is {0}%", cval);
            String ramMesg = String.Format("You are currently using {0}% of your RAM", rval);

            aSpeak(cpuMesg);
            aSpeak(ramMesg);


            int[] cflag = new int[10];
            int i = 0;
            int ix = 0;

            stopWatch.Start();


            while (true)
            {
                cval = (int)cpuCount.NextValue();
                cpu_text.Invoke(new MethodInvoker(setcpu));


                rval = (int)memCount.NextValue();
                ram_text.Invoke(new MethodInvoker(setram));


                if (cval > 10)
                {

                    cflag[i++] = 1;
                    if (i != 0)
                    {

                        if (cflag[i - 1] == 0)
                        {

                            stopWatch.Stop();
                            ts = stopWatch.Elapsed;
                            stopWatch.Reset();
                            stopWatch.Start();
                        }
                    }
                    else if (ix != 0)
                    {
                        if (cflag[9] == 0)
                        {

                            stopWatch.Stop();
                            ts = stopWatch.Elapsed;
                            stopWatch.Reset();
                            stopWatch.Start();
                        }
                    }
                    if (i == 10)
                        i = 0;
                }
                else
                {

                    cflag[i++] = 0;
                    stopWatch.Stop();
                    ts = stopWatch.Elapsed;
                    stopWatch.Reset();
                    stopWatch.Start();
                    if (i == 10)
                        i = 0;
                }
                if (cflag.Sum() == 10)
                {
                    ts = stopWatch.Elapsed;

                    if (ts.Seconds > 10)
                    {
                        mesg = "ALERT!! Your CPU Usage is extremely High";
                        mesg_text.Invoke(new MethodInvoker(setmesg));
                        aSpeak(mesg);

                        mesg = "System will start shutdown process in 10 seconds";
                        mesg_text.Invoke(new MethodInvoker(setmesg));
                        aSpeak(mesg);

                        mesg = "Please save your work and Brace for Impact";
                        mesg_text.Invoke(new MethodInvoker(setmesg));
                        aSpeak(mesg);



                        aSpeak("10");

                        aSpeak("9");

                        aSpeak("8");

                        aSpeak("7");

                        aSpeak("6");

                        aSpeak("5");

                        aSpeak("4");

                        aSpeak("3");

                        aSpeak("2");

                        aSpeak("1");


                        Process cmd = new Process();
                        cmd.StartInfo.FileName = "cmd.exe";
                        cmd.StartInfo.RedirectStandardInput = true;
                        cmd.StartInfo.RedirectStandardOutput = true;
                        cmd.StartInfo.CreateNoWindow = true;
                        cmd.StartInfo.UseShellExecute = false;
                        cmd.Start();

                        cmd.StandardInput.WriteLine("shutdown /l");

                        cmd.StandardInput.Flush();
                        cmd.StandardInput.Close();
                        cmd.WaitForExit();
                        Console.WriteLine(cmd.StandardOutput.ReadToEnd());

                    }
                }
                ix++;
                Thread.Sleep(100);
            }

        }

        public void setcpu()
        {
            cpu_text.Text = cval.ToString();
        }

        public void setram()
        {
            ram_text.Text = rval.ToString();
        }

        public void setmesg()
        {
            mesg_text.Text = mesg;
        }

        public static void aSpeak(String mesg)
        {
            synth.Speak(mesg);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Capimon
{
    public class Memory
    {
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hHandle, IntPtr baseAddress, byte[] buffer, int size, ref int NumberOfBytesRead);

        public Process Target;
        
        bool getHandle(string name)
        {
             Process[] p_;

            bool isNameValid = !String.IsNullOrEmpty(name);
            if (isNameValid)
            {
                p_ = Process.GetProcessesByName(name);

                bool isExistProcess = p_.Length > 0;
                if (isExistProcess)
                {
                    Target = p_[0];

                    return true;
                }
            }

            return false;
        }

        public int init()
        {
            string processName_ = "CapMain";
            int output;

            output = validCheck(processName_);

            return output;
        }

        int validCheck(string pName)
        {
            if (getHandle(pName))
            {
                bool checkWindwoTitle = String.Equals(Target.MainWindowTitle, "Capitalism Lab");

                if(checkWindwoTitle)
                {
                    return 0;
                }

                return 2;
            }
            return 1;
        }

        public string setMsg(int num)
        {
            string result;

            switch (num)
            {
                case 0:
                    result = String.Format("Success! PID: {0}", this.Target.Id);
                    break;
                case 1:
                    result = String.Format("Error {0}: Process가 없음.", num);
                    break;
                case 2:
                    result = String.Format("Error {0}: Process의 Main Window가 맞지 않음", num);
                    break;
                default:
                    result = String.Format("Error 100: 알 수 없는 오류");
                    break;
            }

            return result;
        }

        public byte[] watching(int address, int size)
        {
            IntPtr hHandle = Target.Handle;
            IntPtr baseAddress = Target.MainModule.BaseAddress;
            
            IntPtr targetAddr = baseAddress + address;
            byte[] buffer = new byte[size];
            int reads = 0;

            if (ReadProcessMemory(hHandle, targetAddr, buffer, buffer.Length, ref reads))
            {
                return buffer;
            }

            return buffer;
        }
    }
}
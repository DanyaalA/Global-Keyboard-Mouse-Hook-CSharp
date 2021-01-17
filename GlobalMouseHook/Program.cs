using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GlobalMouseHook
{
    class Program
    {
        static void KeyboardChecker()
        {

            while (true)
            {
                bool keyPress = false;

                var keys = Enum.GetValues(typeof(Key));
                int specKey = 0;
                
                foreach (int key in keys)
                {
                    try
                    {
                        var keyInt = (int)key;
                        var key2 = (Key)Enum.Parse(typeof(Key), keyInt.ToString());

                        keyPress = (Keyboard.GetKeyStates(key2) & KeyStates.Down) > 0;
                        if (keyPress)
                        {
                            specKey = key;
                            break;
                        }
                            
                    }
                    catch
                    {
                        keyPress = false;
                    }
                    
                }



                //switch (x)
                //{


                //    case "LALT":
                //        keyPress = (Keyboard.GetKeyStates(Key.LeftAlt) & KeyStates.Down) > 0;
                //        //Console.WriteLine("Alt Key Pyshed");
                //        break;
                //    //case "MOUSE1":
                //    //    keyPress = Peripherals.Mouse.MouseButtonIsDown(1);
                //    //    break;
                //    //case "MOUSE2":
                //    //    keyPress = Peripherals.Mouse.MouseButtonIsDown(3);
                //    //    break;
                //    //case "MOUSE3":
                //    //    keyPress = Peripherals.Mouse.MouseButtonIsDown(2);
                //    //    break;
                //    //case "XBUTTON1":
                //    //    keyPress = Peripherals.Mouse.MouseButtonIsDown(4);
                //    //    break;
                //    //case "XBUTTON2":
                //    //    keyPress = Peripherals.Mouse.MouseButtonIsDown(5);
                //    //    break;
                //}


               

                if (keyPress)
                {
                    var keyStr = ((Key)specKey).ToString();
                    //Console.WriteLine(keyStr);
                }




                //if ((Keyboard.GetKeyStates(Key.Space) & KeyStates.Down) > 0 && cfg.Bhop)
                //    System.Windows.Forms.SendKeys.SendWait("{L}");

                //if (CBDV.Peripherals.Mouse.MouseButtonIsDown(1))
                //Console.WriteLine("Mouise Down");

                //if ((Keyboard.GetKeyStates(Key.LeftAlt) & KeyStates.Down) > 0)
                Thread.Sleep(1);
            }

        }

        static void Main(string[] args)
        {
          
            Program p = new Program();
            Thread mainThread = new Thread(() => {
                KeyboardChecker();
            });

            mainThread.SetApartmentState(ApartmentState.STA);
            mainThread.Start();
            
            Console.Read();
        }
    }
}

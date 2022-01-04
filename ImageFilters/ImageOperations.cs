using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;



namespace ImageFilters
{
    public class ImageOperations
    {
        /// <summary>
        /// Open an image, convert it to gray scale and load it into 2D array of size (Height x Width)
        /// </summary>
        /// <param name="ImagePath">Image file path</param>
        /// <returns>2D array of gray values</returns>
        public static byte[,] OpenImage(string ImagePath)
        {
            Bitmap original_bm = new Bitmap(ImagePath);
            int Height = original_bm.Height;
            int Width = original_bm.Width;

            byte[,] Buffer = new byte[Height, Width];

            unsafe
            {
                BitmapData bmd = original_bm.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, original_bm.PixelFormat);
                int x, y;
                int nWidth = 0;
                bool Format32 = false;
                bool Format24 = false;
                bool Format8 = false;

                if (original_bm.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    Format24 = true;
                    nWidth = Width * 3;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format32bppArgb || original_bm.PixelFormat == PixelFormat.Format32bppRgb || original_bm.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    Format32 = true;
                    nWidth = Width * 4;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    Format8 = true;
                    nWidth = Width;
                }
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (y = 0; y < Height; y++)
                {
                    for (x = 0; x < Width; x++)
                    {
                        if (Format8)
                        {
                            Buffer[y, x] = p[0];
                            p++;
                        }
                        else
                        {
                            Buffer[y, x] = (byte)((int)(p[0] + p[1] + p[2]) / 3);
                            if (Format24) p += 3;
                            else if (Format32) p += 4;
                        }
                    }
                    p += nOffset;
                }
                original_bm.UnlockBits(bmd);
            }

            return Buffer;
        }

       
        public static int GetHeight(byte[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(0);
        }

        public static int GetWidth(byte[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(1);
        }

        
        public static void DisplayImage(byte[,] ImageMatrix, PictureBox PicBox)
        {
            // Create Image:
            //==============
            int Height = ImageMatrix.GetLength(0);
            int Width = ImageMatrix.GetLength(1);

            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            unsafe
            {
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                int nWidth = 0;
                nWidth = Width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        p[0] = p[1] = p[2] = ImageMatrix[i, j];
                        p += 3;
                    }

                    p += nOffset;
                }
                ImageBMP.UnlockBits(bmd);
            }
            PicBox.Image = ImageBMP;
        }



      
        public static int partition(byte[] Array, int start, int end) //O(N) where N = arrrayLength 
        {
            int i = start + 1; // O(1)
            int piv = Array[start];        // O(1)     //make the first element as pivot element.
            for (int j = start + 1; j <= end; j++)//O(N) where N = arrrayLength 
            {
                /*rearrange the array by putting elements which are less than pivot
                   on one side and which are greater that on other. */

                if (Array[j] < piv) // O(1) 
                {
                    
                    byte temp = Array[i];// O(1)
                    Array[i] = Array[j];// O(1)
                    Array[j] = temp;// O(1)
                    i += 1;
                }
            }
          
            byte temp2 = Array[start]; // O(1)
            Array[start] = Array[i-1]; // O(1) 
            Array[i-1] = temp2;// O(1) 
            return i - 1;          // O(1)             //return the position of the pivot
        }

        //Recursive Part + Non Recursive  , Rec : 2T(n/2) , Non : O(N)  , using master method  Θ(NlogN) Case 2 
        public static byte[] QUICK_SORT(byte[] Array, int start, int end) 
        {
            if (start < end)// O(1)
            {
                //stores the position of pivot element
                int piv_pos = partition(Array, start, end); //O(N) where N = arrrayLength 
                QUICK_SORT(Array, start, piv_pos - 1);    //sorts the left side of pivot. T(n/2)
                QUICK_SORT(Array, piv_pos + 1, end); //sorts the right side of pivot.T(n/2)
            }

            return Array;// O(1)
        }
        public static void displayArray(byte[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine(array[i]);
            }
        }
        public static byte[] kthElement(byte[] array, int trimValue) //O(N*T) 
        {
            for (int i = trimValue; i > 0; i--)  //O(T) T = trimValue 
            {
                array = getMinMaxArray(array);  //O(N)
            }
            return array;
        }
     
        public static byte[] getMinMaxArray(byte[] array) //O(N)
        {
            byte smallest = array[0];   //O(1)
            byte max = array[0]; //O(1)
            for (int i = 0; i < array.Length; i++) //O(N)   N=maxWindowSize*maxWindowSize
            {
                if (array[i] < smallest)  //O(1)
                {
                    smallest = array[i];  //O(1)
                    continue;  //O(1)
                }
                else if (array[i] > max)   //O(1)
                {
                    max = array[i];  //O(1)
                    continue;  //O(1)

                }
            }
            List<byte> list = new List<byte>(array);  //O(1)
            list.Remove(smallest);  //O(1)
            list.Remove(max);  //O(1)
            array = list.ToArray();  //O(1)
            return array;  //O(1)
        }
        public static int calcAverage(byte[] array)//O(N)
        {
            int sum = 0;//O(1)
            for (int i = 0; i < array.Length; i++)//O(N)
            {
                sum += array[i];//O(1)
            }
            int avg = sum / array.Length;//O(1)
            return avg;//O(1)
        }
        //Time Complexty  O(2Max + 3N) which is order O(Max + N) where max = range between min and max , n = array size
        public static byte[] COUNTING_SORT(byte[] Array, int ArrayLength, byte Max, byte Min)
        {
           
            //Initialize Distinct Values Dictionary
            Dictionary<int, int> dist = new Dictionary<int, int>(); //O(1)
            for (int i = Min; i <= Max; i++) //O(Max) , Θ(Max-Min+1) , Max = max number in array ,  Min = min number in array
                dist.Add(i, 0); //O(1)
            //Setup Dictionary Values 
            for (int i = 0; i < ArrayLength; i++)  //O(N)  N=arrayLenght
                dist[Array[i]]++; //O(1)

            //Copy it to an array
            int[] distincts = new int[dist.Count]; //O(1) 
            dist.Values.CopyTo(distincts, 0); //O(1) 

            //Addition of instances
            for (int i = 1; i < dist.Count; i++) //O(Max) , Θ(Max-Min+1) , Max = max number in array ,  Min = min number in array 
                distincts[i] = distincts[i] + distincts[i - 1]; //O(1) 

            //Initialize Output array
            //List<int> output = new List<int>();
            byte[] output = new byte[ArrayLength]; //O(1) 
            for (int i = 0; i < ArrayLength; i++) //O(N)  N=arrayLenght
                output[i] = 0; //O(1) 

            //Counting Sort Function
            for (int i = 0; i < ArrayLength; i++) //O(N)  N=arrayLenght
            {
                output[distincts[Array[i] - Min] - 1] = Array[i]; //O(1)
                distincts[Array[i] - Min]--;//O(1)
            }
            return output; //O(1)
        }

        public static byte AlphaTrimFilter(byte[,] ImageMatrix, int x, int y, int Max_Window_Size, int Sort, int trimValue)
        {
            
            byte[] Array;//O(1)
            int[] Dx, Dy;//O(1)
            if (Max_Window_Size % 2 != 0)//O(1)
            {
                Array = new byte[Max_Window_Size * Max_Window_Size];//O(1)
                Dx = new int[Max_Window_Size * Max_Window_Size];//O(1)
                Dy = new int[Max_Window_Size * Max_Window_Size];//O(1)
            }
            else
            {
                Array = new byte[(Max_Window_Size + 1) * (Max_Window_Size + 1)];//O(1)
                Dx = new int[(Max_Window_Size + 1) * (Max_Window_Size + 1)];//O(1)
                Dy = new int[(Max_Window_Size + 1) * (Max_Window_Size + 1)];//O(1)
            }
            int Index = 0;//O(1)
            //window of size max window size * max window size
            for (int _y = -(Max_Window_Size / 2); _y <= (Max_Window_Size / 2); _y++)//O(N*N) N= Max Window Size
            {
                for (int _x = -(Max_Window_Size / 2); _x <= (Max_Window_Size / 2); _x++)//O(N) N= Max Window Size
                {
                    Dx[Index] = _x;//O(1)
                    Dy[Index] = _y;//O(1)
                    Index++;//O(1)
                }
            }
            byte Max, Min;//O(1)
            int Sum = 0;//O(1)
            int ArrayLength;//O(1)
            int NewY, NewX, Avg = 0;//O(1)
            Max = 0;//O(1)
            Min = 255;//O(1)
            ArrayLength = 0;//O(1)

            for (int i = 0; i < Max_Window_Size * Max_Window_Size; i++)//O(N*N) N= Max Window Size
            {
                NewY = y + Dy[i];//O(1)
                NewX = x + Dx[i];//O(1)
                if (NewX >= 0 && NewX < GetWidth(ImageMatrix) && NewY >= 0 && NewY < GetHeight(ImageMatrix))//O(1)
                {
                    Array[ArrayLength] = ImageMatrix[NewY, NewX];//O(1)
                    //bageb el max w el min hna  3shan astkhdmo fl counting sort , bastkhdmo tany taht 3shan a3ml trim l awel w akher index fl byte array el esmo Array
                    if (Array[ArrayLength] > Max)//O(1)
                        Max = Array[ArrayLength];//O(1)
                    if (Array[ArrayLength] < Min)//O(1)
                        Min = Array[ArrayLength];//O(1)
                    Sum += Array[ArrayLength];//O(1)

                    ArrayLength++;//O(1)
                }
            }
         
            if (Sort == 0)//O(1)
            {
                

                Array = COUNTING_SORT(Array, ArrayLength, Max, Min); //O(Max + N )


            }
            else if (Sort == 1) //O(1)
            {
                

                 

                Array = kthElement(Array, trimValue); //O(N*T)  

            }

            int avg;//O(1)
            if (Sort == 1)//O(1)
            {
                avg = calcAverage(Array);//O(N)
            }

            else//O(1)
            {
                avg = calcAvg(Array, ArrayLength, Max, Min, Avg, Sum, trimValue);
            }

            

            return (byte)avg;//O(1)
        }


        public static int calcAvg(byte[]Array,int ArrayLength,byte Max,byte Min ,int Avg,int Sum , int trimValue)
        {
            if (trimValue == 0)//O(1)
            {
                return Sum /Array.Length;//O(1)
            }
          
            Min = Array[0];//O(1)
            Max = Array[ArrayLength - 1];//O(1)

            byte[] newArray = new byte[Array.Length - 2];//O(1)
            Buffer.BlockCopy(Array, 1, newArray, 0, ArrayLength-2);//O(1)

            Sum -= Min;//O(1)
            Sum -= Max;//O(1)
            ArrayLength -= 2;//O(1)
            trimValue -= 1;//O(1)
            return calcAvg(newArray, ArrayLength, Max, Min, Sum / newArray.Length, Sum, trimValue );
            // Non recursive + recursive = O(1) + T(N-2)  , O(N-2k)

        }



        public static byte AdaptiveFilter(byte[,] ImageMatrix, int x, int y, int W, int Max_Window_Size, int Sort)
        {

            byte[] Array = new byte[W * W]; //O(1)
            int[] Dx = new int[W * W];  //O(1)
            int[] Dy = new int[W * W];  //O(1)
            int Index = 0;
            for (int _y = -(W / 2); _y <= (W / 2); _y++)  //O(N*N) N=windowSize
            {
                for (int _x = -(W / 2); _x <= (W / 2); _x++) //O(N) N=windowSize 
                {
                    Dx[Index] = _x; //O(1) 
                    Dy[Index] = _y;//O(1) 
                    Index++;//O(1) 
                }
            }
            byte Max, Min, Med, Z; //O(1)
            int A1, A2, B1, B2, ArrayLength, NewY, NewX; //O(1)
            Max = 0;  //O(1)
            Min = 255; //O(1)
            ArrayLength = 0; //O(1) 
            Z = ImageMatrix[y, x]; //O(1)
            for (int i = 0; i < W * W; i++)  //O(W*W) W=windowSize
            {
                NewY = y + Dy[i]; //O(1)
                NewX = x + Dx[i]; //O(1)
                if (NewX >= 0 && NewX < GetWidth(ImageMatrix) && NewY >= 0 && NewY < GetHeight(ImageMatrix)) //O(1)
                {
                    Array[ArrayLength] = ImageMatrix[NewY, NewX]; //O(1)
                    if (Array[ArrayLength] > Max) //O(1)
                        Max = Array[ArrayLength]; //O(1)
                    if (Array[ArrayLength] < Min) //O(1)
                        Min = Array[ArrayLength]; //O(1)
                    ArrayLength++; //O(1)
                }
            }

            if (Sort == 0)  //O(1)
                Array = COUNTING_SORT(Array, ArrayLength, Max, Min); //O(Max + N ) 
            else if (Sort == 2)//O(1)
            {
                Array = QUICK_SORT(Array, 0, ArrayLength-1 ); //Θ(NlogN)
            }

            Min = Array[0]; //O(1)
            Med = Array[ArrayLength / 2]; //O(1)
            A1 = Med - Min; //O(1)
            A2 = Max - Med; //O(1)

            if (A1 > 0 && A2 > 0) //O(1)
            {
                B1 = Z - Min; //O(1)
                B2 = Max - Z; //O(1)
                if (B1 > 0 && B2 > 0) //O(1)
                    return Z; //O(1)
                else //O(1)
                    return Med;


            }
            else
            {

                W += 2; //O(1)
                if (W <= Max_Window_Size) //O(1)
                {
                    return AdaptiveFilter(ImageMatrix, x, y, W, Max_Window_Size, Sort); //T(N) = O(N*N) + T(N+2)
                }
                else //O(1)
                {
                    return Med; //O(1)
                }
            }
        }

        public static byte[,] MakeFilter(byte[,] ImageMatrix, int Max_Size, int Sort, int filter, int trimValue, string OpenedFilePath)
        {
            byte[,] ImageMatrix2 = ImageMatrix;

            Bitmap img = new Bitmap(OpenedFilePath);
            for (int y = 0; y < GetHeight(ImageMatrix); y++)
            {
                for (int x = 0; x < GetWidth(ImageMatrix); x++)
                {
                    if (filter == 0)
                        ImageMatrix2[y, x] = AlphaTrimFilter(ImageMatrix, x, y, Max_Size, Sort, trimValue);
                    else
                    {
                        Color pixel = img.GetPixel(x, y);
                        if (pixel.R <= 50 && pixel.G <= 50 && pixel.B <= 50 || pixel.R >= 150 && pixel.G >= 150 && pixel.B >= 150)
                        {
                            ImageMatrix2[y, x] = AdaptiveFilter(ImageMatrix, x, y, 3, Max_Size, Sort);
                        }
                        else
                            continue;


                    }
                }
            }

            return ImageMatrix2;
        }
    }

}

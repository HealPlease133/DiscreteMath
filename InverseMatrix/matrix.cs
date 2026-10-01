using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InverseMatrix
{
    internal class matrix
    {
        public static void Main(string[] args)
        {
            try
            {
                //정방행렬의 크기 입력
                Console.Write("정방행렬의 차수를 입력하세요: ");
                int matSize = int.Parse(Console.ReadLine());

                //크기로 음수가 들어왔을 때
                if(matSize <= 0)
                {
                    throw new Exception($"행렬의 차수는 1 이상의 정수여야 합니다.");
                }


                //정방행렬 생성
                double[,] mat = new double[matSize, matSize];
                string strMat = "";
                for (int i = 0; i < matSize; i++)
                {
                    Console.Write($"{i + 1}행: ");
                    strMat = Console.ReadLine();
                    string[] tempStr = strMat.Split(' ');

                    //행 입력 사이의 공백이 있을 때
                    foreach (string str in tempStr)
                    {
                        if (str == "")
                        {
                            throw new Exception("행렬의 값 사이에 불필요한 공백이 있습니다.");
                        }
                    }

                    //행렬의 크기 만큼 값이 주어지지 않을때
                    if (tempStr.Length != matSize)
                    {
                        throw new IndexOutOfRangeException($"행렬의 크기가 맞지 않습니다.");
                    }

                    for (int j = 0; j < matSize; j++)
                    {
                        mat[i, j] = double.Parse(tempStr[j]);
                    }
                }

                Console.WriteLine();

                //행렬식을 이용한 역행렬 구하기
                double[,] iMat1 = Determinant_InverseMatrix(mat, matSize);

                //가우소-조던 소거법으로 역행렬 구하기
                double[,] iMat2 = Gauss_Jordan_Elimination(mat, matSize);

                //두 역행렬의 결과 비교
                bool isEqual = CompareMatrix(iMat1, iMat2, matSize);

                Console.WriteLine();

                Console.WriteLine($"두 방법의 결과가 동일합니까? : {isEqual}");
            }
            //정방행렬의 크기와 행을 입력할 때 문자열이 들어왔을 때
            catch (FormatException e)
            {
                Console.WriteLine("숫자를 입력해 주세요.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }







            //행렬식을 이용해 역행렬 구하는 함수
            static double[,] Determinant_InverseMatrix(double[,] mat, int matSize)
            {
                //여인수 행렬 구하기
                double[,] cofactorMat = new double[matSize, matSize];
                for (int i = 0; i < matSize; i++)
                {
                    for (int j = 0; j < matSize; j++)
                    {
                        cofactorMat[i, j] = Cofactor(MinorMatrix(i, j, mat), i, j);
                    }
                }

                double det = Determinant(cofactorMat, mat); //여인수 정렬을 통해 행렬식 구하기
                double[,] adjointMat = Adjoint(cofactorMat, matSize); //여인수행렬을 수반행렬로 바꾸기
                double[,] inverseMat = Inverse(adjointMat, det, matSize); //역행렬 구하기

                //행렬식으로 구한 역행렬
                Console.WriteLine($"행렬식으로 구한 역행렬");
                for (int i = 0; i < matSize; i++)
                {
                    for (int j = 0; j < matSize; j++)
                    {
                        inverseMat[i, j] = (inverseMat[i, j] == 0) ? 0 : inverseMat[i, j]; //-0 방지
                        Console.Write($"{Math.Round(inverseMat[i, j], 4)}\t");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine();

                return inverseMat;
            }

            //소행렬 구하기
            static double[,] MinorMatrix(int row, int col, double[,] mat)
            {
                int size = mat.GetLength(0);
                double[,] minor = new double[size - 1, size - 1];
                List<double> nums = new List<double>();
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        nums.Add(mat[i, j]);
                    }
                    for (int j = col + 1; j < size; j++)
                    {
                        nums.Add(mat[i, j]);
                    }
                }
                for (int i = row + 1; i < size; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        nums.Add(mat[i, j]);
                    }
                    for (int j = col + 1; j < size; j++)
                    {
                        nums.Add(mat[i, j]);
                    }
                }
                int rowIndex = 0;
                int colIndex = 0;
                foreach (double i in nums)
                {
                    if (colIndex >= size - 1)
                    {
                        colIndex = 0;
                        rowIndex++;
                    }
                    minor[rowIndex, colIndex++] = i;
                }
                //소행렬 출력 테스트
                //for (int k = 0; k < minor.GetLength(0); k++)
                //{
                //    for (int l = 0; l < minor.GetLength(1); l++)
                //    {
                //        Console.Write($"{minor[k, l]} ");
                //    }
                //    Console.WriteLine();
                //}
                return minor;
            }

            //소행렬식을 통한 여인수 구하기
            static double Cofactor(double[,] minor, int row, int col)
            {
                double det = DeterminantRecursive(minor);
                //switch (minor.GetLength(0))
                //{
                //    //소행렬이 없을때
                //    case 0:
                //        det = 1;
                //        break;
                //    //소행렬이 1 x 1 행렬일떄
                //    case 1:
                //        det = minor[0, 0];
                //        break;
                //    //소행렬이 2 x 2 행렬일떄
                //    case 2:
                //        det = (minor[0, 0] * minor[1, 1]) - (minor[0, 1] * minor[1, 0]);
                //        break;
                //    //소행렬이 3 x 3 행렬일때
                //    case 3:
                //        det = ((minor[0, 0] * minor[1, 1] * minor[2, 2]) + (minor[0, 1] * minor[1, 2] * minor[2, 0]) + (minor[0, 2] * minor[2, 1] * minor[1, 0])) - ((minor[0, 2] * minor[1, 1] * minor[2, 0]) + (minor[1, 2] * minor[2, 1] * minor[0, 0]) + (minor[2, 2] * minor[1, 0] * minor[0, 1]));
                //        break;
                //}

                double cofactor = (double)Math.Pow(-1, row + col) * det;
                return cofactor;
            }

            //수반행렬로 바꾸기
            static double[,] Adjoint(double[,] mat, int size)
            {
                double[,] tempmat = new double[size, size];
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        tempmat[j, i] = mat[i, j];
                    }
                }

                return tempmat;
            }

            //역행렬 구하기
            static double[,] Inverse(double[,] adjointmat, double det, int size)
            {
                double[,] inversemat = new double[size, size];
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        inversemat[i, j] = adjointmat[i, j] / det;
                    }
                }
                return inversemat;
            }

            //여인수 전개를 통한 행렬식 반환
            static double Determinant(double[,] cofactormat, double[,] mat)
            {
                double det = 0;
                for (int i = 0; i < mat.GetLength(0); i++)
                {
                    det += cofactormat[0, i] * mat[0, i];
                }
                if (det == 0)
                {
                    throw new Exception($"det의 값이 0입니다. 현재 det : {det}");
                }
                return det;
            }

            //행렬의 크기에 상관없이 재귀적으로 행렬식 반환
            static double DeterminantRecursive(double[,] mat)
            {
                int matSize = mat.GetLength(0);
                if (matSize == 0)
                {
                    return 1;
                }
                else if (matSize == 1)
                {
                    return mat[0, 0];
                }
                else if (matSize == 2)
                {
                    return (mat[0, 0] * mat[1, 1]) - (mat[0, 1] * mat[1, 0]);
                }

                double det = 0;
                //3x3 이상의 행렬은 소행렬을 생성하여 재귀적으로 행렬식을 계산한다.
                for (int i = 0; i < matSize; i++)
                {
                    double[,] minor = MinorMatrix(0, i, mat);
                    double cofactor = Math.Pow(-1, i) * DeterminantRecursive(minor);
                    det += cofactor * mat[0, i];
                }

                return det;

            }

            //가우소-조던 소거법을 이용해 역행렬 구하는 함수
            static double[,] Gauss_Jordan_Elimination(double[,] mat, int matSize)
            {

                //단위행렬 생성
                double[,] identifyMat = IdentifyMatrix(matSize);

                //가우스-조던 소거법
                UpperTriangular(matSize, ref mat, ref identifyMat);
                MakeIdentityMartrix(matSize, ref mat, ref identifyMat);

                //가우스-조던 방법으로 구한 역행렬
                Console.WriteLine($"가우스-조던 소거법으로 구한 역행렬");
                for (int i = 0; i < matSize; i++)
                {
                    for (int j = 0; j < matSize; j++)
                    {
                        identifyMat[i, j] = (identifyMat[i, j] == 0) ? 0 : identifyMat[i, j]; //-0 방지
                        Console.Write($"{Math.Round(identifyMat[i, j], 4)}\t");
                    }
                    Console.WriteLine();
                }

                return identifyMat;
            }

            //주어진 행렬의 크기에 맞는 단위행렬 생성
            static double[,] IdentifyMatrix(int matSize)
            {
                double[,] identifymat = new double[matSize, matSize];
                for(int i = 0; i < matSize; i++)
                {
                    for(int j = 0; j < matSize; j++)
                    {
                        if (i == j)
                        {
                            identifymat[i, j] = 1;
                        }
                    }
                }
                return identifymat;
            }

            //행렬의 위치 변경
            static void Swap(int cur, int target, int size, ref double[,] mat, ref double[,] identifymat)
            {
                
                for(int i = 0; i < size; i++)
                {
                    double temp = mat[cur, i];
                    mat[cur, i] = mat[target, i];
                    mat[target, i] = temp;
                    temp = identifymat[cur, i];
                    identifymat[cur, i] = identifymat[target, i];
                    identifymat[target, i] = temp;
                }
                return;
            }

            //시작 자리가 0이 아닌 행 찾기
            static int FindNonZero(int i, int matSize, double[,] mat)
            {
                int index = i;
                while (index + 1 < matSize)
                {
                    if (mat[index, i] != 0)
                    {
                        break;
                    }
                    index++;
                }
                //단위행렬을 만들지 못할 때
                if (mat[index, i] == 0)
                {
                    throw new Exception("역행렬이 존재하지 않습니다.");
                }
                return index;
            }

            //피벗 값을 1로 만들기
            static void NormalizePivot(int i, int matSize, ref double[,] mat, ref double[,] identifymat)
            {
                double pivot = mat[i, i];
                for (int k = 0; k < matSize; k++)
                {
                    mat[i, k] /= pivot;
                    identifymat[i, k] /= pivot;
                }
            }

            //상삼각행렬 만들기
            static void UpperTriangular(int matSize, ref double[,] mat, ref double[,] identifyMat)
            {
                //아래로 이동
                for (int i = 0; i < matSize; i++)
                {
                    //시작 자리가 0일떄 0이 아닌 값이 나올때 까지 아래로 이동
                    if (mat[i, i] == 0)
                    {
                        //시작 자리가 0이 아닌 행 찾기
                        int index = FindNonZero(i, matSize, mat);
                        //행렬의 위치 변경
                        Swap(i, index, matSize, ref mat, ref identifyMat);
                    }
                    //피벗이 1이 아닐때 1로 만들기
                    if (mat[i, i] != 1)
                    {
                        NormalizePivot(i, matSize, ref mat, ref identifyMat);
                    }
                    for (int j = i + 1; j < matSize; j++)
                    {
                        //각 행에 빼야할 값 구하기
                        double[] num = new double[matSize];
                        for (int k = 0; k < matSize; k++)
                        {
                            num[k] = mat[i, k] * mat[j, i];
                            //단위행렬 계산
                            identifyMat[j, k] -= (identifyMat[i, k] * mat[j, i]);
                        }
                        for (int k = 0; k < matSize; k++)
                        {
                            //각 행 값 빼기
                            mat[j, k] -= num[k];
                        }
                    }
                }
            }

            //상삼각행렬을 단위행렬로 바꾸기
            static void MakeIdentityMartrix(int matSize, ref double[,] mat, ref double[,] identifyMat)
            {
                for(int i = matSize - 1; i >= 0; i--)
                {
                    //피벗이 1이 아닐때 1로 만들기
                    if (mat[i, i] != 1)
                    {
                        NormalizePivot(i, matSize, ref mat, ref identifyMat);
                    }
                    for (int j = i - 1; j >= 0; j--)
                    {
                        //각 행에 빼야할 값 구하기
                        double[] num = new double[matSize];
                        for (int k = 0; k < matSize; k++)
                        {
                            num[k] = mat[i, k] * mat[j, i];
                            //단위행렬 계산
                            identifyMat[j, k] -= (identifyMat[i, k] * mat[j, i]);
                        }
                        for (int k = 0; k < matSize; k++)
                        {
                            //각 행 값 빼기
                            mat[j, k] -= num[k];
                        }
                    }
                }
            }

            //행렬 값 비교
            static bool CompareMatrix(double[,] mat1, double[,] mat2, int matSize)
            {
                //허용 오차 값
                double epsilon = 0.0001;

                for(int i = 0;  i < matSize; i++)
                {
                    for(int j = 0; j < matSize; j++)
                    {
                        //두 행렬 값의 차의 절댓값이 허용 오차보다 크거나 같은지 확인
                        if (Math.Abs(mat1[i,j] - mat2[i,j]) >= epsilon)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationMatrix
{
    internal class RelationMat
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("5x5 관계 행렬을 입력하세요. (0 또는 1)");
            //초기 행렬 생성
            int[,] initMat = new int[5,5];
            bool isEqu = false;
            //입력값 받아오기
            for(int i = 0; i < 5; i++)
            {
                while (true)
                {
                    Console.Write($"{i + 1}번째 행 입력:");
                    string input = Console.ReadLine();
                    //빈 입력 검사
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("입력값이 비어 있습니다. 다시 입력하세요.");
                        continue;
                    }
                    string[] rows = input.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                    //입력 개수 확인
                    if(rows.Length != 5)
                    {
                        Console.WriteLine("숫자 5개를 입력해야 합니다. 다시 입력하세요.");
                        continue;
                    }
                    int[] temp = new int[5];
                    bool pass = true;
                    for (int j = 0; j < 5; j++)
                    {
                        //숫자가 아닌 값이 들어왔는지 확인
                        if (!int.TryParse(rows[j], out temp[j]))
                        {
                            Console.WriteLine("숫자가 아닌 값이 포함되어 있습니다. 다시 입력하세요.");
                            pass = false;
                            break;
                        }
                        //0또는 1인지 확인
                        if (temp[j] != 0 && temp[j] != 1)
                        {
                            Console.WriteLine("0또는 1만 입력할 수 있습니다. 다시 입력하세요.");
                            pass = false;
                            break;
                        }
                    }
                    if (!pass)
                    {
                        continue;
                    }

                    for (int j = 0; j < 5; j++)
                    {
                        initMat[i,j] = temp[j];
                    }

                    break;
                }
            }
            //초기 관계행렬의 성질 출력
            printRelationProperties(initMat);
            //동치가 될때 까지 반복
            Console.WriteLine("동치 관계 검사");
            while (!isEqu)
            {
                //반사, 대칭, 추이관계 확인 후 전부 맞으면 true 반환 하나라도 틀리면 틀린 관계와 false 반환
                isEqu = isEquivalence(initMat);
            }
            Console.WriteLine("현재 동치 관계입니다.");
            //현재 관계행렬 출력
            printMatrix(initMat);
            //현재 관계행렬의 성질 출력
            printRelationProperties(initMat);
            //동치류 출력
            printEquivalenceClasses(initMat);
        }

        //반사관계 검사
        static bool isReflexive(int[,] mat)
        {
            for (int i = 0; i < 5; i++)
            {
                if (mat[i, i] == 0)
                {
                    return false;
                }
            }
            return true;
        }
        //비반사관계 검사
        static bool isIrreflexive(int[,] mat)
        {
            for (int i = 0; i < 5; i++)
            {
                if (mat[i, i] == 1)
                {
                    return false;
                }
            }
            return true;
        }
        //대칭관계 검사
        static bool isSymmetric(int[,] mat)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (mat[i, j] != mat[j, i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        //반대칭관계 검사
        static bool isAntisymmetric(int[,] mat)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i != j && mat[i, j] == 1 && mat[j, i] == 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        //추이관계 검사
        static bool isTransitive(int[,] mat)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (mat[i, j] == 1 && mat[j, k] == 1 && mat[i, k] == 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        //관계의 성질 출력
        static void printRelationProperties(int[,] mat)
        {
            Console.WriteLine($"반사관계: {(isReflexive(mat) ? "O" : "X")}");
            Console.WriteLine($"비반사관계: {(isIrreflexive(mat) ? "O" : "X")}");
            Console.WriteLine($"대칭관계: {(isSymmetric(mat) ? "O" : "X")}");
            Console.WriteLine($"반대칭관계: {(isAntisymmetric(mat) ? "O" : "X")}");
            Console.WriteLine($"추이관계: {(isTransitive(mat) ? "O" : "X")}");
        }
        //동치 관계 판별
        //static bool isEquivalence(int[,] mat)
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        for (int j = 0; j < 5; j++)
        //        {
        //            //반사관계 확인
        //            if (i == j &&  mat[i,j] == 0)
        //            {
        //                Console.WriteLine($"반사관계가 아닙니다. : ( {i + 1} , {j + 1} )이 존재하지 않습니다. ");
        //                Console.WriteLine("반사폐포 변환 전");
        //                printMatrix(mat);
        //                getReflexiveClosure(mat);
        //                Console.WriteLine("반사폐포 변환 후");
        //                printMatrix(mat);
        //                return false;
        //            }
        //            //대칭관계 확인
        //            if (mat[i,j] == 1 && mat[j,i] != 1)
        //            {
        //                Console.WriteLine($"대칭관계가 아닙니다. : ( {j + 1} , {i + 1} )이 존재하지 않습니다. ");
        //                Console.WriteLine("대칭폐포 변환 전");
        //                printMatrix(mat);
        //                getSymmetricClosure(mat);
        //                Console.WriteLine("대칭폐포 변환 후");
        //                printMatrix(mat);
        //                return false;
        //            }
        //        }
        //    }
        //    //추이관계 확인
        //    int[,] tempMat = new int[5,5];
        //    Array.Copy(mat, tempMat, mat.Length);
        //    Warshall(tempMat);
        //    for (int i = 0; i < 5; i++)
        //    {
        //        for (int j = 0; j < 5; j++)
        //        {
        //            if (mat[i,j] != tempMat[i,j])
        //            {
        //                Console.WriteLine($"추이관계가 아닙니다. : ( {i + 1} , {j + 1} )이 존재하지 않습니다.");
        //                Console.WriteLine("추이폐포 변환 전");
        //                printMatrix(mat);
        //                Warshall(mat);
        //                Console.WriteLine("추이폐포 변환 후");
        //                printMatrix(mat);
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}
        //동치 관계 판별
        static bool isEquivalence(int[,] mat)
        {
            //반사관계 확인
            if (!isReflexive(mat))
            {
                Console.WriteLine("반사관계가 아닙니다.");
                Console.WriteLine("반사폐포 변환 전");
                printMatrix(mat);
                getReflexiveClosure(mat);
                Console.WriteLine("반사폐포 변환 후");
                printMatrix(mat);
                return false;
            }
            //대칭관계 확인
            if (!isSymmetric(mat))
            {
                Console.WriteLine("대칭관계가 아닙니다.");
                Console.WriteLine("대칭폐포 변환 전");
                printMatrix(mat);
                getSymmetricClosure(mat);
                Console.WriteLine("대칭폐포 변환 후");
                printMatrix(mat);
                return false;
            }
            //추이관계 확인
            if (!isTransitive(mat))
            {
                Console.WriteLine("추이관계가 아닙니다.");
                Console.WriteLine("추이폐포 변환 전");
                printMatrix(mat);
                Warshall(mat);
                Console.WriteLine("추이폐포 변환 후");
                printMatrix(mat);
                return false;
            }

            return true;
        }
        //와샬 알고리즘
        static int[,] Warshall(int[,] mat)
        {
            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    for(int k = 0; k < 5; k++)
                    {
                        if (mat[j,k] == 0 && mat[j,i] == 1 && mat[i,k] == 1)
                        {
                            mat[j,k] = 1;
                        }
                    }
                }
            }
            return mat;
        }
        //반사폐포 생성
        static int[,] getReflexiveClosure(int[,] mat)
        {
            for (int i = 0; i < 5; i++)
            {
                mat[i, i] = 1;
            }
            return mat;
        } 
        //대칭폐포 생성
        static int[,] getSymmetricClosure(int[,] mat)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (mat[i,j] == 1)
                    {
                        mat[j, i] = 1;
                    }
                }
            }
            return mat;
        }
        //Matrix 출력
        static void printMatrix(int[,] mat)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Console.Write($"{mat[i,j]} ");
                }
                Console.WriteLine();
            }
            return;
        }
        //동치류 출력
        static void printEquivalenceClasses(int[,] mat)
        {
            List<List<int>> equClass = new List<List<int>>();
            for(int i = 0; i  < 5; i++)
            {
                int[] iEquClass = new int[5];
                List<int> value = new List<int>();
                for(int j=0; j < 5; j++)
                {
                    if (mat[i, j] == 1)
                    {
                        iEquClass[j] = 1;
                        value.Add(j + 1);
                    }
                }
                Console.Write($"[{i + 1}]의 동치류 : ");
                for (int j = 0; j < 5; j++)
                {
                    if (iEquClass[j] == 1)
                    {
                        Console.Write($"{j + 1} ");
                    }
                }
                Console.WriteLine();
                if (!equClass.Any(x => x.SequenceEqual(value)))
                {
                    equClass.Add(value);
                }
            }
            Console.WriteLine($"A의 동치류 : {equClass.Count}개 ");
            foreach(List<int> i in equClass)
            {
                Console.Write("{ ");
                foreach(int j in i)
                {
                    Console.Write($"{j} ");
                }
                Console.WriteLine("}");
            }
            return;
        }
    }
}

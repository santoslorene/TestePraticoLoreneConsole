using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePraticoLoreneConsole
{
    internal class Program
    {
        private static List<Rotas> rotas;

        private static void Main(string[] args)
        {
            CarregaRotas();

            //1. A distância da rota A-B-C
            string[] Exec1 = new string[] { "AB", "BC" };
            Console.WriteLine(string.Format("#1) A distância da rota A-B-C: {0}", RetornaDistancia(Exec1)));

            //2. A distância da rota A-D.
            string[] Exec2 = new string[] { "AD" };
            Console.WriteLine(string.Format("#2) A distância da rota A-D: {0}", RetornaDistancia(Exec2)));

            //3. A distância da rota A-D-C.
            string[] Exec3 = new string[] { "AD", "DC" };
            Console.WriteLine(string.Format("#3) A distância da rota A-D-C: {0}", RetornaDistancia(Exec3)));

            //4. A distância da rota A-E-B-C-D.
            string[] Exec4 = new string[] { "AE", "EB", "BC", "CD" };
            Console.WriteLine(string.Format("#4) A distância da rota A-E-B-C-D: {0}", RetornaDistancia(Exec4)));

            //5. A distância da rota A-E-D.
            string[] Exec5 = new string[] { "AE", "ED" };
            Console.WriteLine(string.Format("#5) A distância da rota A-E-D: {0}", RetornaDistancia(Exec5)));

            //6. O número de viagens começando em C e terminando em C com no máximo 3 paradas.Baseado no
            //contexto apresentado, serão 2 rotas possíveis: C - D - C (2 paradas) e C-E-B-C(3 paradas).
            Questao6(rotas);

            //7. O numero de viagens começando em A e terminando em C com exatamente 4 paradas. Baseado no
            //contexto apresentado, serão 3 rotas possíveis: A para C(via B, C, D); A para C(via D, C, D);
            //e A para C (via D, E, B).
            Questao7(rotas);

            //8. O tamanho da menor viagem(em termos de distância) de A para C.
            Questao8(rotas);

            //9. O tamanho da menor viagem(em termos de distância) de B para B.
            Questao9(rotas);

            //10. O numero de viagens começando em C e terminando em C com distância menor que 30. Baseado no
            //contexto apresentado, serão as rotas seguintes: C-D-C, C-E-B-C, C-E-B-C-D-C, C-D-C-E-B-C, C-D-E-B-C,
            //C-E-B-C-E-B-C e C-E-B-C-E-B-C-E-B-C.

            Console.ReadKey();
        }

        public class Rotas
        {
            public Rotas()
            {
            }

            public string rota { get; set; }
            public int distancia { get; set; }

            public Rotas(string rota, int distancia)
            {
                this.rota = rota;
                this.distancia = distancia;
            }
        }

        public static void CarregaRotas()
        {
            rotas = new List<Rotas>();

            rotas.Add(new Rotas("AB", 5));
            rotas.Add(new Rotas("BC", 4));
            rotas.Add(new Rotas("CD", 8));
            rotas.Add(new Rotas("DC", 8));
            rotas.Add(new Rotas("DE", 6));
            rotas.Add(new Rotas("AD", 5));
            rotas.Add(new Rotas("CE", 2));
            rotas.Add(new Rotas("EB", 3));
            rotas.Add(new Rotas("AE", 7));
        }

        public static string RetornaDistancia(string[] Exercicio)
        {
            int resultadoSoma = 0;

            foreach (var item in Exercicio)
            {
                try
                {
                    var distanciaAtual = rotas.Where(x => x.rota.Contains(item)).FirstOrDefault();
                    resultadoSoma += distanciaAtual.distancia;
                }
                catch (Exception)
                {
                    return string.Format("Rota não existente!");
                }
            }
            return resultadoSoma.ToString();
        }

        //pega a proxima rota, de acordo com o fim da rota enviada
        public static Rotas proximaRota(List<Rotas> rotas, Rotas rota, string Final)
        {
            Rotas a = null;
            foreach (var item in rotas)
            {
                if (item.rota.StartsWith(Final))
                {
                    a = item;
                    break;
                }
            }

            return a;
        }

        public static void Questao6(List<Rotas> rotas)
        {
            List<Rotas> viagensComecandoC = new List<Rotas>();
            int count = 0;

            //monta as listas das rotas começadas e terminadas em C
            foreach (var item in rotas)
            {
                if (item.rota.StartsWith("C"))
                    viagensComecandoC.Add(item);
            }

            //  define o numero das rotas possiveis
            List<Rotas> rotapossivel = new List<Rotas>();
            int flag = 0;
            foreach (var item in viagensComecandoC)
            {
                Rotas rotaAnterior = item;
                if (flag == 0)
                    rotapossivel.Add(item);
                foreach (var item2 in rotas)
                {
                    //se a proxima rota existir
                    if (proximaRota(rotas, rotaAnterior, rotaAnterior.rota.Substring(item.rota.Count() - 1)) != null)
                    {
                        //se a rota não exister na lisa de rota ainda, entra no if
                        if (rotapossivel.Contains(rotaAnterior) == true)
                        {
                            var b = proximaRota(rotas, rotaAnterior, rotaAnterior.rota.Substring(item.rota.Count() - 1));
                            //se a proxima rota, já existir na rota, para o looop.
                            if (rotapossivel.Contains(b))
                                break;
                            rotapossivel.Add(b);
                            rotaAnterior = b;
                            //verifica fim da rota com rotas terminada em C
                            if (b.rota.EndsWith("C"))
                                break;
                        }
                    }
                }
                if (rotapossivel.Count <= 3)
                    count++;
                rotapossivel = new List<Rotas>();
            }
            Console.WriteLine(string.Format("#6) O número de viagens começando em C e terminando em C com no máximo 3 paradas: {0}", count));
        }

        private static void Questao7(List<Rotas> rotas)
        {
            List<Rotas> viagensComecandoC = new List<Rotas>();
            int count = 0;

            //monta as listas das rotas começadas em A e terminadas em C
            foreach (var item in rotas)
            {
                if (item.rota.StartsWith("A"))
                    viagensComecandoC.Add(item);
            }

            //  define o numero das rotas possiveis
            List<Rotas> rotapossivel = new List<Rotas>();
            int flag = 0;
            foreach (var item in viagensComecandoC)
            {
                Rotas rotaAnterior = item;
                if (flag == 0)
                    rotapossivel.Add(item);
                foreach (var item2 in rotas)
                {
                    //se a proxima rota existir
                    if (proximaRota(rotas, rotaAnterior, rotaAnterior.rota.Substring(item.rota.Count() - 1)) != null)
                    {
                        //se a rota não exister na lisa de rota ainda, entra no if
                        if (rotapossivel.Contains(rotaAnterior) == true)
                        {
                            var b = proximaRota(rotas, rotaAnterior, rotaAnterior.rota.Substring(item.rota.Count() - 1));
                            //se a proxima rota, já existir na rota, para o looop.
                            if (rotapossivel.Contains(b))
                                break;
                            rotapossivel.Add(b);
                            rotaAnterior = b;
                            //verifica fim da rota com rotas terminada em C
                            if (b.rota.EndsWith("C"))
                                break;
                        }
                    }
                }
                //if (rotapossivel.Count <= 3)
                count++;
                rotapossivel = new List<Rotas>();
            }
            Console.WriteLine(string.Format("#7) O numero de viagens começando em A e terminando em C com exatamente 4 paradas: {0}", count));
        }

        private static void Questao8(List<Rotas> rotas)
        {
            List<Rotas> viagensComecandoC = new List<Rotas>();
            List<int> menorRota = new List<int>();

            //monta as listas das rotas começadas em A e terminadas em C
            foreach (var item in rotas)
            {
                if (item.rota.StartsWith("A"))
                    viagensComecandoC.Add(item);
            }

            //  define o numero das rotas possiveis
            List<Rotas> rotapossivel = new List<Rotas>();
            int flag = 0;
            foreach (var item in viagensComecandoC)
            {
                Rotas rotaAnterior = item;
                if (flag == 0)
                    rotapossivel.Add(item);
                foreach (var item2 in rotas)
                {
                    //se a proxima rota existir
                    if (proximaRota(rotas, rotaAnterior, rotaAnterior.rota.Substring(item.rota.Count() - 1)) != null)
                    {
                        //se a rota não exister na lisa de rota ainda, entra no if
                        if (rotapossivel.Contains(rotaAnterior) == true)
                        {
                            var b = proximaRota(rotas, rotaAnterior, rotaAnterior.rota.Substring(item.rota.Count() - 1));
                            //se a proxima rota, já existir na rota, para o looop.
                            if (rotapossivel.Contains(b))
                                break;
                            rotapossivel.Add(b);
                            rotaAnterior = b;
                            //verifica fim da rota com rotas terminada em C
                            if (b.rota.EndsWith("C"))
                            {
                                menorRota.Add(rotapossivel.Sum(x => x.distancia));
                                break;
                            }
                        }
                    }
                }
                rotapossivel = new List<Rotas>();
            }
            Console.WriteLine(string.Format("#8) O tamanho da menor viagem(em termos de distância) de A para C: {0}", menorRota.Min()));
        }

        public static Rotas ProximaRotaCurta(List<Rotas> rotasAtual, Rotas rotaAnterior, string Final, List<Rotas> rotapossivel)
        {
            List<Rotas> rota = new List<Rotas>();
            foreach (var item in rotasAtual.Except(rotapossivel))
            {
                if (item.rota.StartsWith(Final))
                {
                    rota.Add(item);
                }
            }
            return rota.FirstOrDefault(x => x.distancia == rota.Min(r => r.distancia));
        }

        public static void Questao9(List<Rotas> rotas)
        {
            List<Rotas> viagensComecandoB = new List<Rotas>();
            List<int> menorRota = new List<int>();

            //monta as listas das rotas começadas em A e terminadas em C
            foreach (var item in rotas)
            {
                if (item.rota.StartsWith("B"))
                    viagensComecandoB.Add(item);
            }

            //  define o numero das rotas possiveis
            List<Rotas> rotapossivel = new List<Rotas>();
            int flag = 0;
            foreach (var item in viagensComecandoB)
            {
                Rotas rotaAnterior = item;
                if (flag == 0)
                    rotapossivel.Add(item);
                foreach (var item2 in rotas)
                {
                    var proximaRota = ProximaRotaCurta(rotas, rotaAnterior, rotaAnterior.rota.Substring(item.rota.Count() - 1), rotapossivel);
                    //se a proxima rota existir
                    //se a rota não exister na lisa de rota ainda, entra no if
                    if (proximaRota != null && rotapossivel.Contains(rotaAnterior))
                    {
                        //se a proxima rota, já existir na rota, para o looop.
                        if (rotapossivel.Contains(proximaRota))
                            break;

                        rotapossivel.Add(proximaRota);
                        rotaAnterior = proximaRota;

                        //verifica fim da rota com rotas terminada em C
                        if (proximaRota.rota.EndsWith("B"))
                        {
                            menorRota.Add(rotapossivel.Sum(x => x.distancia));
                            break;
                        }
                    }
                }
                rotapossivel = new List<Rotas>();
            }
            Console.WriteLine(string.Format("#9) O tamanho da menor viagem(em termos de distância) de B para B: {0}", menorRota.Min()));
        }
    }
}
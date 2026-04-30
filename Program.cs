// Intento de Wordle
// Por Ayrton D Sella, 30/04/2026
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

internal class Word
{
    // Lista de palabras a adivinar
    // Originalmente un enum, pero terminé pasando a un array de strings
    static string[] Words =
    {
        "comer", "vivir", "tener", "hacer", "poder", "decir", "salir", "venir", "mirar", "tomar", "dejar", "ganar", "beber", "abrir", "subir", "bajar", "creer", "andar", "crear", "traer", "perro", "campo", "playa", "cielo", "suelo", "barco", "silla", "llave", "coche", "avion", "motor", "rueda", "pared", "techo", "queso", "fuego", "hielo", "dulce", "fruta", "carne", "arroz", "pasta", "hojas", "nubes", "brisa", "calor", "noche", "tarde", "sueño", "grito", "canto", "baile", "danza", "ritmo", "libro", "papel", "tinta", "pluma", "carta", "radio", "video", "fotos", "serie", "drama", "humor", "miedo", "verde", "negro", "blanco", "claro", "suave", "leves", "corto", "largo", "ancho", "altos", "bajar", "justo", "pobre", "ricos", "sabio", "tonto", "feliz", "serio", "lento", "sucio", "secar", "mojar", "tibio", "joven", "viejo", "cavar", "cazar", "tejer", "coser", "lavar", "pesar", "medir", "jugar", "saber", "valer", "doler", "mover", "poner", "robar", "sudar", "pagar", "besar", "rezar", "nadar", "tocar", "sacar", "tapar", "picar", "rotar", "fumar", "girar", "jalar", "untar", "regar", "cegar", "pegar", "ligar", "mundo", "nieve", "plaza", "calle", "pasto", "arena", "selva", "costa", "norte", "oeste", "sudor", "color", "forma", "texto", "punto", "valor", "orden", "grupo", "nivel", "clima", "corte", "honor", "error", "curso", "broma", "falta", "lista", "tabla", "metal", "acero", "plomo", "cobre", "lente", "vista", "brazo", "dedos", "caras", "audio", "raton", "leona", "tigre", "cebra", "burro", "cabra", "oveja", "cerdo", "zorro", "abeja", "mosca", "plaga", "hongo", "alamo", "roble", "cedro", "lirio", "rosas", "delta", "llano", "valle", "senda", "bahia", "islas", "lagos", "pilar", "sabor", "lugar", "tarea"
    };

    // Generador de número aleatorio
    // Originalmente estaba dentro de main, pasó a WordGen y ahora es estático dentro de la clase
    static Random r = new Random();
    public static string WordGen()
    {
        string wordToGuess = Words[r.Next(0, Words.Length)];
        return wordToGuess.ToUpper();
    }

    // En caso de que alguien quiera volver a leer las instrucciones siempre podemos hacer una llamada a este método
    public static void Instructions()
    {
        System.Console.WriteLine("""

        ===== ¡¡HOLA!! ===== 
        Bienvenido a mi Wordle
        Te explicaré cómo funciona esto...

        Tienes que adivinar una palabra de 5 letras en 5 intentos.
        Cada letra saldrá remarcada con un color distintos conforme las uses.
        """);
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.Green;
        System.Console.WriteLine("LETRAS VERDES = Letra correcta en posición correcta");
        Console.ResetColor();
        Console.BackgroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("LETRAS AMARILLAS = Letra correcta en posición equivocada");
        Console.ResetColor();
        Console.BackgroundColor = ConsoleColor.Red;
        System.Console.WriteLine("LETRAS ROJAS = Letra equivocada");
        Console.ResetColor();
    }

    // Comienzo del juego, si se pone otra cosa a S o N lanza excepción propia
    public static bool Start()
    {
        while (true)
        {
            System.Console.Write("¿¿Comenzamos?? (S / N) ");
            string entry = Console.ReadLine()!.ToUpper();
            if (entry == "S")
                return true;
            else if (entry == "N")
                return false;

            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("Error en los parámetros de entrada (Debes escribir S o N)");
            Console.ResetColor();
        }
    }
    // Petición de palabra al usuario
    public static string Input()
    {
        while (true)
        {
            System.Console.Write("Danos una palabra de 5 letras: ");
            string inputWord = Console.ReadLine()!.ToUpper();

            if (inputWord.Length == 5)
                return inputWord;

            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("La palabra debe tener exactamente 5 letras");
            Console.ResetColor();
        }
    }

    // Comparador letra por letra de la palabra a adivinar con la entrada del usuario
    public static void Comparing(string secret, string input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == secret[i])
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (secret.Contains(input[i]))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            Console.Write(input[i]);
            Console.ResetColor();
        }
        System.Console.WriteLine();
    }
    // Creamos el teclado
    static Dictionary<char, ConsoleColor> keyboard = new Dictionary<char, ConsoleColor>();
    public static void initKeyboard()
    {
        for (char c = 'A'; c <= 'Z'; c++)
        {
            keyboard[c] = ConsoleColor.Gray;
        }
    }
    // Actualiza el teclado conforme los intentos, marca de verde y amarillo las letras encontradas en la palabra y de gris las usadas que no
    public static void UpdateKeyboard(string secret, string input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (c == secret[i])
                keyboard[c] = ConsoleColor.Green;
            else if (secret.Contains(c))
            {
                if (keyboard[c] != ConsoleColor.Green)
                    keyboard[c] = ConsoleColor.Yellow;
            }
            else
            {
                if (keyboard[c] != ConsoleColor.Green && keyboard[c] != ConsoleColor.Yellow)
                    keyboard[c] = ConsoleColor.DarkGray;
            }
        }
    }
    public static void PrintRow(char start, char end)
    {
        for (char c = start; c <= end; c++)
        {
            Console.BackgroundColor = keyboard[c];
            Console.ForegroundColor = ConsoleColor.Black;

            Console.Write($" {c} ");
            Console.ResetColor();
            System.Console.Write(" ");
        }
        System.Console.WriteLine("\n");
    }
    public static void PrintKeyboard()
    {
        PrintRow('A', 'M');
        PrintRow('N', 'Z');
        System.Console.WriteLine("\n");
    }

    // Clase principal que se encarga de llamar a todos los métodos para que funcione el juego
    private static void Main(string[] args)
    {
        Instructions();
        if (Start())
        {
            string secret = WordGen();
            // System.Console.WriteLine(secret);
            System.Console.WriteLine("\nYa tenemos tu palabra preparada, puedes comenzar a adivinar");
            initKeyboard();
            bool won = false;

            int maxTries = 5;
            for (int i = 1; i <= maxTries; i++)
            {
                string inp = Input();
                Comparing(secret, inp);
                UpdateKeyboard(secret, inp);
                if (inp == secret)
                {
                    won = true;
                    Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.WriteLine("\n===== ✅ ¡¡HAS GANADO!! ✅ =====");
                    System.Console.WriteLine($"\nHas tardado {i} intentos");
                    Console.ResetColor();
                    break;
                }
                PrintKeyboard();
                System.Console.WriteLine($"Intento nº {i} de 5");
            }
            if (!won)
                System.Console.WriteLine($"\nHas perdido, la palabra era: {secret}\n");
        }
    }
}
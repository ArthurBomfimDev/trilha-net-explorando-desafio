using System.Runtime.CompilerServices;
using System.Text;
using DesafioProjetoHospedagem.Models;

Console.OutputEncoding = Encoding.UTF8;

// Listas para armazenar os dados em "memória" durante a execução do programa
List<Suite> listSuites = new List<Suite>();
List<Reserva> listReservas = new List<Reserva>();

ExibirMenu();


void ExibirMenu()
{
    bool executar = true;
    while (executar)
    {
        Console.Clear();
        Console.WriteLine("================================================");
        Console.WriteLine("\tSistema de Hospedagem");
        Console.WriteLine("================================================");
        Console.WriteLine("Escolha uma opção:");
        Console.WriteLine("1 - Cadastrar Suítes");
        Console.WriteLine("2 - Fazer Check-In e Reservar");
        Console.WriteLine("3 - Sair");
        Console.WriteLine("------------------------------------------------");
        Console.Write("Sua escolha: ");

        switch (Console.ReadLine())
        {
            case "1":
                CadastrarSuite();
                break;
            case "2":
                RealizarCheckInEReserva();
                break;
            case "3":
                executar = false;
                Console.WriteLine("Encerrando o sistema...");
                break;
            default:
                Console.WriteLine("Opção inválida. Pressione qualquer tecla para tentar novamente.");
                Console.ReadKey();
                break;
        }
    }
}

void RealizarCheckInEReserva()
{
    Console.Clear();
    if (listSuites.Count == 0)
    {
        Console.WriteLine("!!! ATENÇÃO !!!");
        Console.WriteLine("Nenhuma suíte foi cadastrada ainda.");
        Console.WriteLine("Por favor, cadastre ao menos uma suíte antes de fazer uma reserva.");
        Console.WriteLine("\nPressione qualquer tecla para voltar ao menu principal.");
        Console.ReadKey();
        return;
    }

    // Etapa 1: Realizar o Check-in dos Hóspedes
    List<Pessoa> hospedes = CheckIn();

    // Etapa 2: Listar Suítes com capacidade suficiente
    Console.Clear();
    Console.WriteLine("------------------------------------------------");
    Console.WriteLine("     Selecione uma Suíte Disponível");
    Console.WriteLine("------------------------------------------------");

    var suitesDisponiveis = new List<Suite>();
    for (int i = 0; i < listSuites.Count; i++)
    {
        if (listSuites[i].Capacidade >= hospedes.Count)
        {
            Suite suite = listSuites[i];
            Console.WriteLine($" {suitesDisponiveis.Count + 1} | Tipo: {suite.TipoSuite}, Capacidade: {suite.Capacidade}, Valor Diária: {suite.ValorDiaria:C}");
            suitesDisponiveis.Add(suite);
        }
    }

    if (suitesDisponiveis.Count == 0)
    {
        Console.WriteLine("\nNão há suítes disponíveis com capacidade para a quantidade de hóspedes informada.");
        Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal.");
        Console.ReadKey();
        return;
    }

    // Etapa 3: Escolha da Suíte
    int escolhaSuite = 0;
    Suite suiteEscolhida = null;
    while (suiteEscolhida == null)
    {
        Console.Write("\nEscolha o número da suíte: ");
        int.TryParse(Console.ReadLine(), out escolhaSuite);

        if (escolhaSuite > 0 && escolhaSuite <= suitesDisponiveis.Count)
        {
            suiteEscolhida = suitesDisponiveis[escolhaSuite - 1];
        }
        else
        {
            Console.WriteLine(" * Escolha inválida. Digite um dos números listados acima.");
        }
    }

    // Etapa 4: Definir dias da reserva
    int diasReservados = 0;
    while (diasReservados <= 0)
    {
        Console.Write("Digite a quantidade de dias da reserva: ");
        int.TryParse(Console.ReadLine(), out diasReservados);
        if (diasReservados <= 0)
        {
            Console.WriteLine(" * O número de dias deve ser maior que zero.");
        }
    }

    // Etapa 5: Criar e confirmar a reserva
    try
    {
        Reserva novaReserva = new Reserva(diasReservados);
        novaReserva.CadastrarSuite(suiteEscolhida);
        novaReserva.CadastrarHospedes(hospedes);
        listReservas.Add(novaReserva); // Adiciona a reserva a uma lista de reservas geral

        Console.Clear();
        Console.WriteLine("================================================");
        Console.WriteLine("\tRESERVA CONFIRMADA COM SUCESSO!");
        Console.WriteLine("================================================");
        Console.WriteLine($" Hóspedes: {novaReserva.ObterQuantidadeHospedes()}");
        Console.WriteLine($" Suíte: {novaReserva.Suite.TipoSuite}");
        Console.WriteLine($" Dias Reservados: {novaReserva.DiasReservados}");
        Console.WriteLine($"------------------------------------------------");
        Console.WriteLine($" VALOR TOTAL: {novaReserva.CalcularValorDiaria():C}");
        if (novaReserva.DiasReservados >= 10)
        {
            Console.WriteLine(" (Desconto de 10% aplicado para 10 ou mais diárias)");
        }
        Console.WriteLine("\nPressione qualquer tecla para voltar ao menu principal.");
        Console.ReadKey();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nOcorreu um erro ao criar a reserva: {ex.Message}");
        Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal.");
        Console.ReadKey();
    }
}


void CadastrarSuite()
{
    Console.Clear();
    Console.WriteLine("================================================");
    Console.WriteLine("\t\tCadastro de Suites");
    Console.WriteLine("================================================");

    int quantidadeSuites = 0;

    while (quantidadeSuites <= 0)
    {
        Console.Write(" * Quantidade de Suites que deseja cadastrar: ");
        int.TryParse(Console.ReadLine(), out quantidadeSuites);

        if (quantidadeSuites <= 0)
            Console.WriteLine(" * Insira um Número inteiro maior do que 0");
    }
    for (int i = 0; i < quantidadeSuites; i++)
    {
        Console.Clear();
        Console.WriteLine("-------------------");
        Console.WriteLine($" -x- Suite N°{i + 1} -x-");
        Console.WriteLine("-------------------");

        Suite suite = new();

        while (suite.TipoSuite == default)
        {
            Console.WriteLine(" Tipo de Suite");
            Console.WriteLine(" 1 - Common");
            Console.WriteLine(" 2 - Premium");
            Console.WriteLine(" 3 - Master");
            Console.WriteLine("-------------------");
            Console.Write(" Sua Escolha: ");
            int.TryParse(Console.ReadLine(), out int tipo);

            if (tipo > 0 && tipo < 4)
                suite.TipoSuite = ((EnumTipoSuite)tipo).ToString();
            else
                Console.WriteLine(" * Digite Apenas Números de 1 a 3");
            Console.WriteLine("-------------------");
        }

        while (suite.Capacidade == default)
        {
            Console.Write("Digite a Capacidade da Suite: ");
            int.TryParse(Console.ReadLine(), out int capacidade);

            if (capacidade > 0)
                suite.Capacidade = capacidade;
            else
                Console.WriteLine(" * Digite Apenas Números inteiros maiores do que 0");
            Console.WriteLine("-------------------");
        }

        while (suite.ValorDiaria == default)
        {
            Console.Write("Digite o valor da diaria R$: ");
            decimal.TryParse(Console.ReadLine(), out decimal valorDiaria);

            if (valorDiaria > 0)
                suite.ValorDiaria = valorDiaria;
            else
                Console.WriteLine(" * Digite Apenas Números maiores do que 0");
            Console.WriteLine("-------------------");
        }

        listSuites.Add(suite);
        Console.WriteLine("\nSuíte cadastrada com sucesso!");
    }
    Console.WriteLine("\nCadastro de suítes finalizado. Pressione qualquer tecla para voltar ao menu.");
    Console.ReadKey();
}

List<Pessoa> CheckIn()
{
    List<Pessoa> listHospedes = new();
    Console.Clear();
    Console.WriteLine("================================");
    Console.WriteLine("\t    Check-In");
    Console.WriteLine("================================");

    int quantidadeHospedes = 0;

    while (quantidadeHospedes <= 0)
    {
        Console.Write(" * Quantidade de Hospedes: ");
        int.TryParse(Console.ReadLine(), out quantidadeHospedes);

        if (quantidadeHospedes <= 0)
            Console.WriteLine(" * Insira um Número inteiro maior do que 0");
    }

    for (int i = 0; i < quantidadeHospedes; i++)
    {
        Pessoa hospede = new();

        Console.Clear();
        Console.WriteLine("-------------------");
        Console.WriteLine($"-x- Hóspede N°{i + 1} -x-");
        Console.WriteLine("-------------------");

        while (string.IsNullOrEmpty(hospede.Nome))
        {
            Console.Write(" Digite o Nome: ");
            var nome = ToPascalCase(Console.ReadLine());
            if (string.IsNullOrWhiteSpace(nome))
                Console.WriteLine(" * Erro - Digite Novamente seu Nome");
            else
                hospede.Nome = nome;
        }

        while (string.IsNullOrEmpty(hospede.Sobrenome))
        {
            Console.WriteLine("-------------------");
            Console.Write(" Digite o Sobrenome: ");
            var sobrenome = ToPascalCase(Console.ReadLine());
            if (string.IsNullOrWhiteSpace(sobrenome))
                Console.WriteLine(" * Erro - Digite Novamente seu Sobrenome");
            else
                hospede.Sobrenome = sobrenome;
        }
        listHospedes.Add(hospede);
    }

    return listHospedes;
}

string ToPascalCase(string texto)
{
    if (string.IsNullOrWhiteSpace(texto))
        return string.Empty;

    List<string> pascalCaseTexto = new List<string>();

    foreach (var palavra in texto.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
    {
        var novaPalavra = $"{char.ToUpper(palavra[0])}{palavra.Substring(1).ToLower()}";
        pascalCaseTexto.Add(novaPalavra);
    }

    return string.Join(" ", pascalCaseTexto);
}
using System.Text.Json;

internal class Program
{
    public class Transfer
    {
        public int threshold { set; get; }
        public int low_fee { set; get; }
        public int high_fee { set; get; }
    }

    public class Confirmation
    {
        public string en { set; get; }
        public string id { set; get; }
    }

    public class BankTransferConfig
    {
        public string lang { set; get; }
        public Transfer transfer {set; get;}
        public List<String> methods { set; get; }
        public Confirmation confirmation { set; get; }
    }

    public class AppConfig
    {
        public BankTransferConfig config;
        private const string fileConfigPath = "../../../bank_transfer_config.json";

        public AppConfig()
        {
            try
            {
                ReadConfigFile();
            }
            catch
            {
                SetDefault();
                WriteConfigFile();
            }
        }

        public void ReadConfigFile()
        {
            string configJsonData = File.ReadAllText(fileConfigPath);
            config = JsonSerializer.Deserialize<BankTransferConfig>(configJsonData);
        }

        public void WriteConfigFile()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            string data = JsonSerializer.Serialize(config);
            File.WriteAllText(fileConfigPath, data);
        }

        public void SetDefault()
        {
            config = new BankTransferConfig();
            config.lang = "en";
            //config.transfer.threshold = 25000000;
            config.transfer = new Transfer();
            config.transfer.threshold = 25000000;
            config.transfer.low_fee = 6500;
            config.transfer.high_fee = 15000;
            string[] dataMethod = { "RTO (real-time)", "SKN", "RTGS", "BI FAST" };
            config.methods = new List<string>(dataMethod);
            config.confirmation = new Confirmation();
            config.confirmation.en = "yes";
            config.confirmation.id = "ya";

        }
    }

    private static void Main(string[] args)
    {
        AppConfig cfg = new AppConfig();

        if(cfg.config.lang == "en")
        {
            Console.Write("Please insert the amount of money to transfer : ");
        }
        else
        {
            Console.Write("Masukkan jumlah uang yang akan di-transfer : ");
        }

        int uang = int.Parse(Console.ReadLine());
        int biaya = 0;

        if(uang <= cfg.config.transfer.threshold)
        {
            biaya = cfg.config.transfer.low_fee;
        }
        else
        {
            biaya = cfg.config.transfer.high_fee;
        }

        if (cfg.config.lang == "en")
        {
            Console.WriteLine($"Transfer fee = {biaya}");
            Console.WriteLine($"Total amount = {uang + biaya}");

            Console.WriteLine("\nSelect transfer method : ");
        }
        else
        {
            Console.WriteLine($"Biaya transfer = {biaya}");
            Console.WriteLine($"Total biaya = {uang + biaya}");
            Console.WriteLine("Pilih metode transfer : ");
        }

        int i = 1;
        foreach(String d in cfg.config.methods)
        {
            Console.WriteLine($"{i} {d}");
            i++;
        }

        Console.Write("Pilihan : ");
        int idxMethods = int.Parse(Console.ReadLine());

        Console.WriteLine();

        if(cfg.config.lang == "en")
        {
            Console.Write($"Please type {cfg.config.confirmation.en} to confirm the transaction : ");
            string confirm = Console.ReadLine();

            if(confirm == cfg.config.confirmation.en)
            {
                Console.WriteLine("The transfer is completed");
            }
            else
            {
                Console.WriteLine("Transfer is cancelled");
            }

        }
        else
        {
            Console.Write($"Ketik {cfg.config.confirmation.id} untuk mengkonfirmasi transaksi : ");
            string confirm = Console.ReadLine();

            if (confirm == cfg.config.confirmation.id)
            {
                Console.WriteLine("Proses transfer berhasil");
            }
            else
            {
                Console.WriteLine("Transfer dibatalkan");
            }
        }
    }
}
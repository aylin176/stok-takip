using System;
using System.Collections.Generic;
using System.Text;

namespace StokTakipUygulamasi
{
    // Ürün sınıfı: Ürünleri temsil eder.
    public class Urun
    {
        public int UrunId { get; set; }
        public string UrunAdi { get; set; }
        public int StokMiktari { get; set; }

        // Constructor: Ürün ID, ad ve başlangıç stok miktarı alır.
        public Urun(int urunId, string urunAdi, int stokMiktari)
        {
            UrunId = urunId;
            UrunAdi = urunAdi;
            StokMiktari = stokMiktari;
        }

        /// <summary>
        /// Stok miktarını günceller. Pozitif değer ekler, negatif değer çıkartır.
        /// </summary>
        /// <param name="miktar">Güncelleme miktarı</param>
        public void StokGuncelle(int miktar)
        {
            StokMiktari += miktar;
        }

        /// <summary>
        /// Sipariş oluşturulmaya çalışılır. Yetersiz stok durumunda false döner.
        /// </summary>
        /// <param name="miktar">Sipariş edilecek miktar</param>
        /// <returns>İşlem başarılıysa true, aksi halde false.</returns>
        public bool SiparisOlustur(int miktar)
        {
            if (miktar > StokMiktari)
            {
                Console.WriteLine($"Sipariş oluşturulamadı. Yetersiz stok! Mevcut stok: {StokMiktari}");
                return false;
            }
            StokMiktari -= miktar;
            Console.WriteLine($"Sipariş oluşturuldu! Kalan stok: {StokMiktari}");
            return true;
        }

        public override string ToString()
        {
            return $"Ürün ID: {UrunId}, Adı: {UrunAdi}, Stok: {StokMiktari}";
        }
    }

    // Sipariş sınıfı: Sipariş işlemlerine ilişkin bilgileri barındırır.
    public class Siparis
    {
        public int SiparisId { get; set; }
        public Urun SiparisEdilenUrun { get; set; }
        public int SiparisMiktari { get; set; }
        public DateTime SiparisTarihi { get; set; }

        // Constructor: Sipariş ID, sipariş edilen ürün, miktar ve sipariş tarihi oluşturur.
        public Siparis(int siparisId, Urun urun, int miktar)
        {
            SiparisId = siparisId;
            SiparisEdilenUrun = urun;
            SiparisMiktari = miktar;
            SiparisTarihi = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Sipariş ID: {SiparisId}, Ürün: {SiparisEdilenUrun.UrunAdi}, Miktar: {SiparisMiktari}, Tarih: {SiparisTarihi}";
        }
    }

    // StokTakipSistemi sınıfı: Ürün ve siparişleri yöneten ana sınıftır.
    public class StokTakipSistemi
    {
        public List<Urun> UrunListesi { get; set; }
        public List<Siparis> SiparisListesi { get; set; }
        private int nextUrunId = 1;
        private int nextSiparisId = 1;

        // Constructor: Boş listeler ile başlatır.
        public StokTakipSistemi()
        {
            UrunListesi = new List<Urun>();
            SiparisListesi = new List<Siparis>();
        }

        /// <summary>
        /// Yeni ürün ekler: Kullanıcıdan ürün adı ve stok miktarı bilgilerini alır.
        /// </summary>
        public void UrunEkle()
        {
            Console.Write("Ürün Adı: ");
            string urunAdi = Console.ReadLine();

            Console.Write("Başlangıç Stok Miktarı: ");
            int stokMiktari;
            while (!int.TryParse(Console.ReadLine(), out stokMiktari))
            {
                Console.Write("Geçersiz değer. Lütfen stok miktarını sayı olarak giriniz: ");
            }

            Urun yeniUrun = new Urun(nextUrunId++, urunAdi, stokMiktari);
            UrunListesi.Add(yeniUrun);

            Console.WriteLine("\nÜrün başarıyla eklendi:");
            Console.WriteLine(yeniUrun);
        }

        /// <summary>
        /// Sistemdeki tüm ürünleri listeler.
        /// </summary>
        public void UrunleriListele()
        {
            if (UrunListesi.Count == 0)
            {
                Console.WriteLine("Sistemde kayıtlı ürün bulunmamaktadır.");
                return;
            }
            foreach (Urun urun in UrunListesi)
            {
                Console.WriteLine(urun);
            }
        }

        /// <summary>
        /// Kullanıcının ürün stokunu manuel olarak güncellemesini sağlar.
        /// Ürün ID'si ve güncelleme miktarı (pozitif ya da negatif) istenir.
        /// </summary>
        public void UrunStokGuncelle()
        {
            Console.Write("Stok güncellemek istediğiniz Ürün ID giriniz: ");
            int urunId;
            if (!int.TryParse(Console.ReadLine(), out urunId))
            {
                Console.WriteLine("Geçersiz Ürün ID!");
                return;
            }

            Urun urun = UrunListesi.Find(u => u.UrunId == urunId);
            if (urun == null)
            {
                Console.WriteLine("Ürün bulunamadı!");
                return;
            }

            Console.Write("Stok güncelleme için eklemek istiyorsanız pozitif, azaltmak istiyorsanız negatif bir sayı giriniz: ");
            int miktar;
            if (!int.TryParse(Console.ReadLine(), out miktar))
            {
                Console.WriteLine("Geçersiz miktar!");
                return;
            }
            urun.StokGuncelle(miktar);
            Console.WriteLine("Stok güncellendi:");
            Console.WriteLine(urun);
        }

        /// <summary>
        /// Sipariş oluşturmak için ürün ID'si ve sipariş miktarı bilgilerini alır.
        /// Ürün stok yeterliliği kontrol edilip sipariş oluşturulur.
        /// </summary>
        public void SiparisOlustur()
        {
            Console.Write("Sipariş verilecek Ürün ID'si giriniz: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Geçersiz Ürün ID!");
                return;
            }
            Urun urun = UrunListesi.Find(u => u.UrunId == id);
            if (urun == null)
            {
                Console.WriteLine("Ürün bulunamadı.");
                return;
            }

            Console.Write("Sipariş miktarı giriniz: ");
            int miktar;
            if (!int.TryParse(Console.ReadLine(), out miktar))
            {
                Console.WriteLine("Geçersiz sipariş miktarı!");
                return;
            }

            if (urun.SiparisOlustur(miktar))
            {
                Siparis yeniSiparis = new Siparis(nextSiparisId++, urun, miktar);
                SiparisListesi.Add(yeniSiparis);
                Console.WriteLine("\nSipariş başarıyla oluşturuldu:");
                Console.WriteLine(yeniSiparis);
            }
        }

        /// <summary>
        /// Oluşturulmuş tüm siparişlerin bilgisini listeler.
        /// </summary>
        public void SiparisleriListele()
        {
            if (SiparisListesi.Count == 0)
            {
                Console.WriteLine("Henüz sipariş oluşturulmamış.");
                return;
            }
            foreach (Siparis siparis in SiparisListesi)
            {
                Console.WriteLine(siparis);
            }
        }
    }

    // Program sınıfı: Admin giriş sistemi ve ana menünün detaylı kodunu içerir.
    class Program
    {
        static void Main(string[] args)
        {
            // Admin giriş sistemi:
            PerformLogin();

            // Giriş sonrası stok takip sistemi örneği oluşturuluyor.
            StokTakipSistemi sistem = new StokTakipSistemi();

            // Ana menü döngüsü
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n--- Stok Takip Sistemi ---");
                Console.WriteLine("1. Ürün Ekle");
                Console.WriteLine("2. Ürünleri Listele");
                Console.WriteLine("3. Ürün Stok Güncelle");
                Console.WriteLine("4. Sipariş Oluştur");
                Console.WriteLine("5. Siparişleri Listele");
                Console.WriteLine("6. Çıkış");
                Console.Write("Seçiminiz: ");
                string secim = Console.ReadLine();
                Console.WriteLine();

                switch (secim)
                {
                    case "1":
                        sistem.UrunEkle();
                        break;
                    case "2":
                        sistem.UrunleriListele();
                        break;
                    case "3":
                        sistem.UrunStokGuncelle();
                        break;
                    case "4":
                        sistem.SiparisOlustur();
                        break;
                    case "5":
                        sistem.SiparisleriListele();
                        break;
                    case "6":
                        Console.WriteLine("Sistemden çıkılıyor...");
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyiniz.");
                        break;
                }

                Console.WriteLine("\nDevam etmek için bir tuşa basınız...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Adminin (sabit: "admin"/"admin123") giriş yapmasını sağlayan metottur.
        /// Maksimum 3 giriş denemesi tanımaktadır; başarısızlık durumunda program sonlandırılır.
        /// </summary>
        static void PerformLogin()
        {
            int maxAttempts = 3;
            int attempt = 0;
            bool isAuthenticated = false;

            while (attempt < maxAttempts && !isAuthenticated)
            {
                Console.Clear();
                WriteHeader();

                Console.Write("Kullanıcı Adı: ");
                string username = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine("Kullanıcı adı boş olamaz.");
                    attempt++;
                    Console.WriteLine("Devam etmek için bir tuşa basınız...");
                    Console.ReadKey();
                    continue;
                }

                Console.Write("Parola: ");
                string password = ReadPassword();
                if (string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Parola boş olamaz.");
                    attempt++;
                    Console.WriteLine("Devam etmek için bir tuşa basınız...");
                    Console.ReadKey();
                    continue;
                }

                if (username.Equals("admin", StringComparison.OrdinalIgnoreCase) && password == "admin123")
                {
                    isAuthenticated = true;
                    Console.WriteLine("\nYönetici olarak giriş başarılı.");
                    Console.WriteLine("\nGiriş işlemi tamamlandı. Ana menüye yönlendiriliyorsunuz...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nGiriş bilgileri hatalı. Tekrar deneyiniz.");
                    attempt++;
                    if (attempt < maxAttempts)
                    {
                        Console.WriteLine($"Kalan deneme hakkınız: {maxAttempts - attempt}");
                    }
                    Console.WriteLine("Devam etmek için bir tuşa basınız...");
                    Console.ReadKey();
                }
            }

            if (!isAuthenticated)
            {
                Console.WriteLine("Çok fazla hatalı giriş denemesi. Program sonlandırılıyor.");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Giriş ekranı başlığını ekrana yazdırır.
        /// </summary>
        static void WriteHeader()
        {
            Console.WriteLine("***********************************************");
            Console.WriteLine("            Stok Takip Sistemi               ");
            Console.WriteLine("***********************************************\n");
        }

        /// <summary>
        /// Kullanıcının parolayı yıldızlar (*) ile maskeler ve girilen değeri döndürür.
        /// </summary>
        /// <returns>Kullanıcının girdiği şifre</returns>
        static string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password.Remove(password.Length - 1, 1);
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    Console.Write("*");
                    password.Append(key.KeyChar);
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password.ToString();
        }
    }
}
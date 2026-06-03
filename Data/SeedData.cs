using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MythosBalance.Models;

namespace MythosBalance.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            await EnsureRoleAsync(roleManager, "Admin");
            await EnsureRoleAsync(roleManager, "User");

            var adminUser = await userManager.FindByEmailAsync("admin@mythosbalance.com");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@mythosbalance.com",
                    Email = "admin@mythosbalance.com",
                    DisplayName = "Sistem Yöneticisi",
                    Bio = "Mythos Balance platform yöneticisi.",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var testUser = await userManager.FindByEmailAsync("demo@mythosbalance.com");
            if (testUser == null)
            {
                testUser = new ApplicationUser
                {
                    UserName = "demo@mythosbalance.com",
                    Email = "demo@mythosbalance.com",
                    DisplayName = "Demo Kullanıcı",
                    Bio = "Mythos Balance'ı keşfediyorum.",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };
                await userManager.CreateAsync(testUser, "Demo123!");
                await userManager.AddToRoleAsync(testUser, "User");
            }

            if (!await context.LifeDomains.AnyAsync())
            {
                var domains = new List<LifeDomain>
                {
                    new LifeDomain
                    {
                        Name = "Health",
                        TurkishName = "Sağlık",
                        Description = "Bedensel ve zihinsel sağlığınızı korumaya yönelik aktiviteler.",
                        IconClass = "bi bi-heart-pulse",
                        ColorHex = "#e06c75"
                    },
                    new LifeDomain
                    {
                        Name = "Education",
                        TurkishName = "Eğitim",
                        Description = "Öğrenme, okuma ve zihinsel gelişime yönelik aktiviteler.",
                        IconClass = "bi bi-book",
                        ColorHex = "#61afef"
                    },
                    new LifeDomain
                    {
                        Name = "Creativity",
                        TurkishName = "Hobi & Yaratıcılık",
                        Description = "Sanat, müzik ve yaratıcı ifadeye yönelik aktiviteler.",
                        IconClass = "bi bi-palette",
                        ColorHex = "#c678dd"
                    },
                    new LifeDomain
                    {
                        Name = "Travel",
                        TurkishName = "Seyahat",
                        Description = "Keşif, gezi ve yeni yerler deneyimlemeye yönelik aktiviteler.",
                        IconClass = "bi bi-compass",
                        ColorHex = "#56b6c2"
                    },
                    new LifeDomain
                    {
                        Name = "Social",
                        TurkishName = "Sosyal Yaşam",
                        Description = "Aile, arkadaşlık ve toplumsal bağlara yönelik aktiviteler.",
                        IconClass = "bi bi-people",
                        ColorHex = "#e5c07b"
                    }
                };
                await context.LifeDomains.AddRangeAsync(domains);
                await context.SaveChangesAsync();
            }

            if (!await context.MythologyGuides.AnyAsync())
            {
                var healthDomain = await context.LifeDomains.FirstAsync(d => d.Name == "Health");
                var educationDomain = await context.LifeDomains.FirstAsync(d => d.Name == "Education");
                var creativityDomain = await context.LifeDomains.FirstAsync(d => d.Name == "Creativity");
                var travelDomain = await context.LifeDomains.FirstAsync(d => d.Name == "Travel");
                var socialDomain = await context.LifeDomains.FirstAsync(d => d.Name == "Social");

                var guides = new List<MythologyGuide>
                {
                    new MythologyGuide
                    {
                        Name = "Hygieia",
                        Title = "Sağlık ve Koruyucu Hekimlik Tanrıçası",
                        ShortDescription = "Hygieia, Yunan mitolojisinde sağlık, temizlik ve hijyenin tanrıçasıdır. Tıp tanrısı Asklepios'un kızı olan Hygieia, hastalığı önleme ve sağlıklı yaşam pratiğinin simgesidir.",
                        FullDescription = "Hygieia, Yunan panteonunda önemli bir konuma sahip olan sağlık tanrıçasıdır. Babası Asklepios hasta olanları iyileştirirken, Hygieia hastalığın önlenmesine odaklanır; bu yönüyle koruyucu hekimliğin ilk sembolü sayılır. Adı, günümüzde tüm dünyada kullanılan 'hygiene' (hijyen) kelimesinin kaynağıdır.",
                        Symbols = "Yılan, Kase, Defne Dalı, Beyaz Elbise",
                        HistoricalBackground = "Hygieia kültü, antik Yunan'da M.Ö. 5. yüzyıldan itibaren yaygınlaşmıştır. Asklepion tapınaklarında babasıyla birlikte tapınım görmüş, Roma döneminde ise 'Salus' adıyla kültü devam etmiştir. Pek çok antik yazıt ve heykel, elinde yılan sarılı bir kase tutan genç bir kadın figürü olarak tasvir eder.",
                        WhyThisGuide = "Hygieia, sağlık alanını temsil etmek için en uygun mitolojik figürdür; zira odağı tedavi değil önlemedir. Sağlıklı alışkanlıklar edinmek, düzenli egzersiz yapmak ve bedenimize iyi davranmak — bunlar Hygieia'nın bize öğrettiği yaşam felsefesidir.",
                        MythologicalStory = "Efsaneye göre Hygieia, babasıyla birlikte hastane tapınaklarında yılanların bakımını üstlenirmiş. Yılan, yenilenerek deri değiştirmesi nedeniyle iyileşme ve dönüşümün simgesi kabul edilmiştir. Hygieia'nın elindeki kase (Hygieia Kasesi), günümüzde eczacılığın evrensel sembolü olarak yaşamaya devam etmektedir.",
                        ImagePath = "/images/gods/hygieia.png",
                        LifeDomainId = healthDomain.Id,
                        References = new List<GuideReference>
                        {
                            new GuideReference { Title = "Greek Religion", Author = "Walter Burkert", Year = 1985, Publisher = "Harvard University Press" },
                            new GuideReference { Title = "The Cult of Asklepios", Author = "Emma J. Edelstein", Year = 1998, Publisher = "Johns Hopkins University Press" },
                            new GuideReference { Title = "Hygieia — Encyclopedia Britannica", Url = "https://www.britannica.com/topic/Hygieia" }
                        }
                    },
                    new MythologyGuide
                    {
                        Name = "Athena",
                        Title = "Bilgelik, Strateji ve Bilim Tanrıçası",
                        ShortDescription = "Athena, Yunan mitolojisinde bilgelik, strateji, zanaat ve medeniyetin tanrıçasıdır. Zeus'un en sevgili çocuğu olarak bilinen Athena, Atina şehrinin koruyucusu ve öğrenmenin simgesidir.",
                        FullDescription = "Athena, Yunan mitolojisinin en karmaşık ve çok yönlü figürlerinden biridir. Savaş tanrıçası olmasına karşın o, kaba gücü değil stratejiyi, bilgeliği ve adaleti temsil eder. Zanaat, el sanatları, matematik ve felsefe onun himayesindedir. Atina şehri ona adanmış olup Akropolis tepesindeki Parthenon tapınağı onun şerefine inşa edilmiştir.",
                        Symbols = "Baykuş, Zeytin Dalı, Miğfer, Kalkan, Mızrak",
                        HistoricalBackground = "Athena'nın kökeni tartışmalıdır; bazı akademisyenler figürün Miken dönemine kadar uzandığını öne sürmektedir. Homeros'un İlyada ve Odysseia destanlarında kritik roller üstlenen Athena, Olimpos tanrılarının en saygın üyelerinden biri olarak tasvir edilir. Roma mitolojisinde karşılığı 'Minerva'dır.",
                        WhyThisGuide = "Athena, eğitim ve öğrenme alanını temsil eder çünkü bilgelik, merak ve anlayış onun özündedir. Her yeni bilgi edinme çabası, bir kitap okuma eylemi ya da öğrenim yolculuğu Athena'nın ruhunu taşır. Baykuş imgesi, karanlıkta bile görebilmeyi — yani bilginin belirsizliği aydınlatmasını — simgeler.",
                        MythologicalStory = "Efsaneye göre Athena, Zeus'un başından tam zırhlı ve silahlanmış olarak doğmuştur. Atina şehri için Poseidon ile yarışmış; Poseidon denizden tuzlu su fışkırtırken Athena insanlara zeytin ağacı armağan etmiştir. Atinalılar zeytini daha değerli bularak şehri Athena'ya adamıştır.",
                        ImagePath = "/images/gods/athena.png",
                        LifeDomainId = educationDomain.Id,
                        References = new List<GuideReference>
                        {
                            new GuideReference { Title = "Theogony", Author = "Hesiod", Year = null, Publisher = "Antik Yunan" },
                            new GuideReference { Title = "The Iliad", Author = "Homer", Year = null, Publisher = "Antik Yunan" },
                            new GuideReference { Title = "Athena — Ancient History Encyclopedia", Url = "https://www.worldhistory.org/athena/" }
                        }
                    },
                    new MythologyGuide
                    {
                        Name = "Apollo",
                        Title = "Müzik, Şiir ve Güzellik Tanrısı",
                        ShortDescription = "Apollo, Yunan mitolojisinde müzik, şiir, sanat, güzellik, ışık ve kehanet tanrısıdır. Zeus ile Leto'nun oğlu, Artemis'in ikiz kardeşidir ve Olimpos'un en güçlü tanrılarından biri sayılır.",
                        FullDescription = "Apollo, antik çağların en karmaşık ve kültürel açıdan en zengin tanrısıdır. Müzik ve sanatın yanı sıra güneş, ışık, hakikat ve kehanet onun alanlarıdır. Delphi'deki ünlü kehanet merkezi (Oracle) ona adanmıştır. Sanat ve bilimlerin ilham perileri olan Musalar, onun liderliğinde danışılarak çalışır.",
                        Symbols = "Lir, Ok ve Yay, Defne Çelengi, Güneş, Kartal",
                        HistoricalBackground = "Apollo kültü, Anadolu ve Doğu Akdeniz kökenli olduğu düşünülmektedir. M.Ö. 8. yüzyıldan itibaren Yunan dünyasında hızla yayılmış ve Delphi'deki kutsal alanı, antik dünyanın en önemli dini merkezlerinden biri haline gelmiştir. Roma mitolojisinde aynı adla (Apollo) yer alır.",
                        WhyThisGuide = "Apollo, yaratıcılık ve hobiler alanını temsil eder çünkü sanat, müzik, şiir ve güzellik onun özüdür. Her yaratıcı eylem — bir melodi yazmak, fotoğraf çekmek, resim yapmak — Apollo'nun ilham alanına girer. Liri ile müzik icra eden figürü, insanlığın yaratıcı ruhunun en kadim simgelerinden biridir.",
                        MythologicalStory = "Efsaneye göre Apollo ve kız kardeşi Artemis, Hera'nın zulmünden kaçan anneleri Leto tarafından bir adada doğurulmuştur. Apollo, çocukken annesiyle kardeşine bakan ejderha Python'u Delphi'de ok atarak öldürmüş ve bu mücadele, karanlık üzerindeki ışığın zaferini simgelemiştir.",
                        ImagePath = "/images/gods/apollo.png",
                        LifeDomainId = creativityDomain.Id,
                        References = new List<GuideReference>
                        {
                            new GuideReference { Title = "The Homeric Hymns", Author = "Anonymous", Year = null, Publisher = "Antik Yunan" },
                            new GuideReference { Title = "Apollo — Mythology and Cult", Author = "Fritz Graf", Year = 2009, Publisher = "Cambridge University Press" },
                            new GuideReference { Title = "Apollo — World History Encyclopedia", Url = "https://www.worldhistory.org/apollo/" }
                        }
                    },
                    new MythologyGuide
                    {
                        Name = "Hermes",
                        Title = "Yolcuların, Tüccarların ve Habercilerin Tanrısı",
                        ShortDescription = "Hermes, Yunan mitolojisinde yolcuların, tüccarların, hırsızların ve habercilerin tanrısıdır. Olimpos'un en hızlı ve en kurnaz tanrısı olarak sınırları aşan, iki dünyayı birbirine bağlayan bir aracıdır.",
                        FullDescription = "Hermes, Yunan panteonunun en çok yönlü figürlerinden biridir. Ölüler diyarına ruhları götüren psikopompos (ruh rehberi), Olimpos'un habercisi ve Zeus'un elçisidir. Ticaret, hız, iletişim ve seyahat onun alanlarıdır. Kanatları olan sandalet ve başlığı ile anında her yere ulaşabilir.",
                        Symbols = "Kanatli Sandalet, Caduceus (Yılan Sarılı Asa), Petasos (Hasır Şapka), Kese",
                        HistoricalBackground = "Hermes kültü, özellikle yol kavşaklarına ve geçitlere dikilen 'Herm' adlı taş sütunlarla somutlaşmıştır. Antik Atina sokaklarında bu taşlar seyahatin ve iletişimin sembolü olarak yer almıştır. Roma mitolojisinde 'Merkür' adıyla anılır ve ticaret gezegenine bu isim verilmiştir.",
                        WhyThisGuide = "Hermes, seyahat ve keşif alanını temsil eder çünkü sınırları tanımayan, yeni yerleri keşfeden ve her yolculuğu anlam taşıyan figürdür. Bir müzeyi gezmek, yeni bir şehir keşfetmek ya da bilinmeyene adım atmak — bunlar Hermes'in ruhunu taşır.",
                        MythologicalStory = "Efsaneye göre Hermes, doğduğu gün beşiğinden çıkarak Apollo'nun sığır sürüsünü çalmıştır. Suçunu örtbas etmek için Zeus'a giderek savunmasını yapmış, tüm Olimpos tanrılarını güldürmeyi başarmıştır. Sonunda Apollo ile lir aletini takas ederek barışmış ve Olimpos'un resmi habercisi olmuştur.",
                        ImagePath = "/images/gods/hermes.png",
                        LifeDomainId = travelDomain.Id,
                        References = new List<GuideReference>
                        {
                            new GuideReference { Title = "Homeric Hymn to Hermes", Author = "Anonymous", Year = null, Publisher = "Antik Yunan" },
                            new GuideReference { Title = "Hermes: Guide of Souls", Author = "Karl Kerenyi", Year = 1976, Publisher = "Spring Publications" },
                            new GuideReference { Title = "Hermes — Ancient History Encyclopedia", Url = "https://www.worldhistory.org/hermes/" }
                        }
                    },
                    new MythologyGuide
                    {
                        Name = "Charites",
                        Title = "Arkadaşlık, Neşe ve Toplumsal Uyumun Üç Güzeli",
                        ShortDescription = "Khariteler (Charites), Yunan mitolojisinde zarafet, neşe ve toplumsal uyumun üç tanrıçasıdır: Aglaia (Parlaklık), Euphrosyne (Mutluluk) ve Thalia (Bolluk). Birlikte insanları birbirine bağlayan tüm güzel sosyal anların kaynağıdır.",
                        FullDescription = "Khariteler, Yunan mitolojisinde zarafet ve güzelliğin kişileştirilmiş hâlidir. Zeus ile Okyanus kızı Eurynome'nin üç kızı olarak doğmuş olan Aglaia, Euphrosyne ve Thalia; tek başlarına birer figür olsalar da her zaman birlikte, el ele dans ederken tasvir edilirler. Bu üçlü birlik, insan ilişkilerinin özünü yansıtır: vermek, almak ve paylaşmak. Antik Yunan'da sanatçılara ilham verdikleri, şairlerin eserlerine güç kattıkları ve toplumsal uyumun kaynağı oldukları kabul edilirdi.",
                        Symbols = "Üç Figür Dans, Çiçek Çelengi, Sarmaşık, Güneş Işığı, Birleşik Eller",
                        HistoricalBackground = "Khariteler kültü, Antik Yunan'ın en eski katmanlarına uzanır. Hesiodos'un Theogonia'sında adlarıyla zikredilirler. Plastik sanatta en çok rastlanan tasvirleri, el ele tutuşmuş veya omuzdan sarılmış üç çıplak figür biçimindedir — bu imge, güzelliğin ve sevincin döngüsel doğasını simgeler. Roma mitolojisinde 'Gratiae' (Sevgi Tanrıçaları) adıyla anılmış; Rönesans döneminde Botticelli'nin 'Venüs'ün Doğuşu' ve 'İlkbahar' tablolarında ölümsüzleştirilmişlerdir.",
                        WhyThisGuide = "Khariteler, sosyal yaşam alanını temsil etmek için Hestia'dan çok daha uygun bir seçimdir. Hestia'nın odağı ev içi düzen ve aile yuvası iken Khariteler'in özü dışa dönük sosyal etkileşim, dostluk bağları ve toplumsal katılımdır. Sosyal refah, araştırmalar tarafından da desteklendiği üzere tek boyutlu değildir: empati ve nezaketten (Aglaia) paylaşılan neşeye (Euphrosyne) ve topluluk kutlamalarına (Thalia) uzanan çok katmanlı bir yapıdır. Bu proje, Kharitleri sosyal yaşam rehberi olarak benimseyerek uygulamanın mimari sadeliğini korurken sosyal refahı daha zengin ve anlamlı biçimde temsil etmeyi amaçlamaktadır.",
                        MythologicalStory = "Efsaneye göre Khariteler, Olimpos'ta daima Afrodit'e eşlik ederdi; onun giysilerini diker, saçlarını tarar ve tanrıçanın zarafetini tamamlarlardı. Homeros'un Odysseia'sında Uyku Tanrısı Hypnos, Aglaia'ya duyduğu aşkı dile getirir. Başka mitlerde Khariteler, Apollo ve Musalar'ın yanında dans edip şarkı söyler; yaratıcılık ve sanat onların sevinci olmadan anlamsız kalır. Antik şair Pindaros, her başarıyı Khariteler'in armağanı olarak yorumlar: insanın kazanımları ancak paylaşıldığında anlam kazanır.",
                        ImagePath = "/images/gods/charites.png",
                        LifeDomainId = socialDomain.Id,
                        References = new List<GuideReference>
                        {
                            new GuideReference { Title = "Theogony", Author = "Hesiod", Year = null, Publisher = "Antik Yunan" },
                            new GuideReference { Title = "The Homeric Hymns", Author = "Anonymous", Year = null, Publisher = "Antik Yunan" },
                            new GuideReference { Title = "Charites — World History Encyclopedia", Url = "https://www.worldhistory.org/Charites/" }
                        }
                    }
                };

                await context.MythologyGuides.AddRangeAsync(guides);
                await context.SaveChangesAsync();
            }

            if (!await context.Activities.AnyAsync() && testUser != null)
            {
                var healthDomain = await context.LifeDomains.FirstAsync(d => d.Name == "Health");
                var educationDomain = await context.LifeDomains.FirstAsync(d => d.Name == "Education");

                var demoActivities = new List<Activity>
                {
                    new Activity
                    {
                        Title = "Sabah yürüyüşü",
                        Description = "30 dakika parkta yürüyüş yaptım.",
                        Date = DateTime.Today.AddDays(-1),
                        DurationMinutes = 30,
                        UserId = testUser.Id,
                        LifeDomainId = healthDomain.Id,
                        CreatedAt = DateTime.UtcNow.AddDays(-1)
                    },
                    new Activity
                    {
                        Title = "C# kitabı okuma",
                        Description = "Entity Framework Core bölümünü çalıştım.",
                        Date = DateTime.Today.AddDays(-2),
                        DurationMinutes = 60,
                        UserId = testUser.Id,
                        LifeDomainId = educationDomain.Id,
                        CreatedAt = DateTime.UtcNow.AddDays(-2)
                    }
                };

                await context.Activities.AddRangeAsync(demoActivities);
                await context.SaveChangesAsync();
            }
        }

        private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

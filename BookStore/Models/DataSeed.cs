using BookStore.Areas.Identity.Data;
using BookStore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<BookStoreUser>>();
            IdentityResult roleResult;
            var registeredRoleCheck = await RoleManager.RoleExistsAsync("Registered");
            if (!registeredRoleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Registered"));
            }
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            BookStoreUser user = await UserManager.FindByEmailAsync("admin@mvcmovie.com");
            if (user == null)
            {
                var User = new BookStoreUser();
                User.Email = "admin@BookStore.com";
                User.UserName = "admin";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }
            
            // Assign "Registered" Role to all registered users
            var allUsers = await UserManager.Users.ToListAsync();
            foreach (var usr in allUsers)
            {
                if (!await UserManager.IsInRoleAsync(usr, "Registered"))
                {
                    await UserManager.AddToRoleAsync(usr, "Registered");
                }
            }
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BookStoreContext(
                serviceProvider.GetRequiredService<DbContextOptions<BookStoreContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();
                if (context.Books.Any() || context.Author.Any() || context.Genres.Any())
                {
                    return;
                }
                context.Author.AddRange(
             new Author { /*Id = 1, */FirstName = "Sebastian", LastName = "Fitzek", Date = DateTime.Parse("1971-10-13"), Nationality = "German", Gender = "Male" },
             new Author { /*Id = 2, */FirstName = "Stephen", LastName = "King", Date = DateTime.Parse("1947-9-21"), Nationality = "American", Gender = "Male" },
             new Author { /*Id = 3, */FirstName = "J.K.", LastName = "Rowling", Date = DateTime.Parse("1965-7-31"), Nationality = "English", Gender = "Female" }
                 );
                
                context.SaveChanges();

                context.Genres.AddRange(
                new Genres { GenreName = "Thriller" },
                new Genres { GenreName = "Drama" },
                new Genres { GenreName = "Science-Fiction" },
                new Genres { GenreName = "Horror" },
                new Genres { GenreName = "Fantasy" },
                new Genres { GenreName = "Detective" },
                new Genres { GenreName = "Adventure" }
                    );
                context.SaveChanges();

                context.UserBooks.AddRange(
          new UserBooks
          {
              BookId = 1,
              AppUser = "Marko"
          },
          new UserBooks
          {
              BookId = 1,
              AppUser = "Bojan"
          },
          new UserBooks
          {
              BookId = 2,
              AppUser = "Tijana"
          },
          new UserBooks
          {
              BookId = 1,
              AppUser = "Pecka"
          },
          new UserBooks
          {
              BookId = 1,
              AppUser = "Ivona"
          },
          new UserBooks
          {
              BookId = 2,
              AppUser = "Tomi"
          },
          new UserBooks
          {
              BookId = 2,
              AppUser = "Sara"
          },
          new UserBooks
          {
              BookId = 1,
              AppUser = "Stanka"
          }
      );

                

                context.Books.AddRange(
new Books
{
    Title = "Harry Potter and the Philosopher's Stone",
    YearPublished = 1997,
    NumPages = 223,
    Description = "Harry Potter and the Philosopher's Stone is a fantasy novel written by British author J. K. Rowling. The first novel in the Harry Potter series and Rowling's debut novel, it follows Harry Potter, a young wizard who discovers his magical heritage on his eleventh birthday, when he receives a letter of acceptance to Hogwarts School of Witchcraft and Wizardry. Harry makes close friends and a few enemies during his first year at the school and with the help of his friends, Ron Weasley and Hermione Granger, he faces an attempted comeback by the dark wizard Lord Voldemort, who killed Harry's parents, but failed to kill Harry when he was just 15 months old.",
    Publisher = "Bloomsbury UK",
    FrontPage = "https://res.cloudinary.com/bloomsbury-atlas/image/upload/w_148,c_scale/jackets/9781408855652.jpg",
    DownloadUrl = "https://www.amazon.com/Harry-Potter-Philosophers-Stone-Rowling/dp/0747532745",
    AuthorId = 3
},
new Books
{
    Title = "Harry Potter and the Goblet of Fire",
    YearPublished = 2000,
    NumPages = 636,
    Description = "Harry Potter and the Goblet of Fire is a fantasy novel written by British author J. K. Rowling and the fourth novel in the Harry Potter series. It follows Harry Potter, a wizard in his fourth year at Hogwarts School of Witchcraft and Wizardry, and the mystery surrounding the entry of Harry's name into the Triwizard Tournament, in which he is forced to compete.",
    Publisher = "Bloomsbury UK",
    FrontPage = "https://en.wikipedia.org/wiki/Harry_Potter_and_the_Goblet_of_Fire#/media/File:Harry_Potter_and_the_Goblet_of_Fire_cover.png",
    DownloadUrl = "https://www.amazon.com/Harry-Potter-Goblet-Fire-Rowling/dp/0439139600",
    AuthorId = 3
},
new Books
{
    Title = "Fantastic Beasts and Where to Find Them",
    YearPublished = 2001,
    NumPages = 128,
    Description = "Fantastic Beasts and Where to Find Them (often referred to as simply Fantastic Beasts) is a 2001 guide book written by British author J. K. Rowling (under the pen name of the fictitious author Newt Scamander) about the magical creatures in the Harry Potter universe. The original version, illustrated by the author herself, purports to be Harry Potter's copy of the textbook of the same name mentioned in Harry Potter and the Philosopher's Stone (or Harry Potter and the Sorcerer's Stone in the US), the first novel of the Harry Potter series. It includes several notes inside it supposedly handwritten by Harry, Ron Weasley, and Hermione Granger, detailing their own experiences with some of the beasts described, and including inside jokes relating to the original series.",
    Publisher = "Bloomsbury UK and Canada",
    FrontPage = "https://en.wikipedia.org/wiki/Fantastic_Beasts_and_Where_to_Find_Them#/media/File:Fantastic_beasts.JPG",
    DownloadUrl = "https://www.amazon.com/Fantastic-Beasts-Where-Find-Them/dp/1338132318",
    AuthorId = 3
},
new Books
{
    Title = "The Shining",
    YearPublished = 1977,
    NumPages = 447,
    Description = "The Shining centers on Jack Torrance, a struggling writer and recovering alcoholic who accepts a position as the off-season caretaker of the historic Overlook Hotel in the Colorado Rockies. His family accompanies him on this job, including his young son Danny, who possesses \"the shining\", an array of psychic abilities that allow the child to glimpse the hotel's horrific true nature. Soon, after a winter storm leaves the family snowbound, the supernatural forces inhabiting the hotel influence Jack's sanity, leaving his wife and son in grave danger.",
    Publisher = "Doubleday",
    FrontPage = "https://en.wikipedia.org/wiki/The_Shining_(novel)#/media/File:The_Shining_(1977)_front_cover,_first_edition.jpg",
    DownloadUrl = "https://www.amazon.com/Shining-Stephen-King/dp/0307743659",
    AuthorId = 2
},
new Books
{
    Title = "It",
    YearPublished = 1986,
    NumPages = 1138,
    Description = "It is a 1986 horror novel by American author Stephen King. It was his 22nd book and the 17th novel written under his own name. The story follows the experiences of seven children as they are terrorized by an evil entity that exploits the fears of its victims to disguise itself while hunting its prey. \"It\" primarily appears in the form of Pennywise the Dancing Clown to attract its preferred prey of young children.",
    Publisher = "Viking",
    FrontPage = "https://en.wikipedia.org/wiki/It_(novel)#/media/File:It_(1986)_front_cover,_first_edition.jpg",
    DownloadUrl = "https://www.amazon.com/Novel-Stephen-King/dp/1501142976",
    AuthorId = 2
},
new Books
{
    Title = "Pet Sematary",
    YearPublished = 1983,
    NumPages = 374,
    Description = "Louis Creed, a doctor from Chicago, is appointed director of the University of Maine's campus health service. He moves to a house near the town of Ludlow with his wife Rachel, their two young children, Ellie and Gage, and Ellie's cat, Winston Churchill (\"Church\"). Their elderly neighbor, Jud Crandall, warns Louis and Rachel about the highway that runs past their house, which is frequented by speeding trucks.",
    Publisher = "Doubleday",
    FrontPage = "~/Images/images.jfif",
    DownloadUrl = "https://www.amazon.com/Pet-Sematary-Novel-Stephen-King/dp/1501156705",
    AuthorId = 2
},
new Books
{
    Title = "The Eye Collector",
    YearPublished = 2010,
    NumPages = 436,
    Description = "Ready or not, here he comes. He plays the oldest children's game in the world, hide and seek. Only the Eye Collector plays it to death. It is the same each time. A woman's body is found with a ticking stopwatch clutched in her dead hand. A distraught father must find his child before the boy suffocates - and the killer takes his left eye. Alexander Zorbach, a washed-up cop turned journalist has reported all three of the Eye Collector's murders. But this is different. His wallet has been found next to the corpse and now he's a suspect. The Eye Collector wants Zorbach to play. Zorbach has exactly forty-five hours, seven minutes to save a little boy's life. And the countdown has started.",
    Publisher = "Bloomsbury & Corvus",
    FrontPage = "https://m.media-amazon.com/images/I/81NV6CKCn0L._AC_UF894,1000_QL80_.jpg",
    DownloadUrl = "https://www.amazon.co.uk/Eye-Collector-gripping-chilling-psychological/dp/0857893696",
    AuthorId = 1
},
new Books
{
    Title = "The Eye Hunter",
    YearPublished = 2011,
    NumPages = 352,
    Description = "\r\nDr Suker is one of the best eye surgeons in the world. He is also a psychopath who abducts women and removes their eyelids.\r\n\r\nSo far, all the victims of the twisted doctor's crimes have committed suicide shortly thereafter. The police are unable to proceed against him due to the lack of evidence.\r\n\r\nWhen another woman is abducted, her mother turns to Alina Gregoriev for help. Gregoriev, the blind physiotherapist, has been considered a medium since her abilities helped capture an infamous serial killer.\r\n\r\nShe reluctantly gets involved in the Suker case, where she is drawn into a world of madness and violence that also engulfs her old friend, police officer turned journalist Alexander Zorbach.",
    Publisher = "Bloomsbury",
    FrontPage = "https://res.cloudinary.com/bloomsbury-atlas/image/upload/w_568,c_scale/jackets/9781804542378.jpg",
    DownloadUrl = "https://www.amazon.com/Eye-Hunter-Sebastian-Fitzek/dp/1804542385",
    AuthorId = 1
},
new Books
{
    Title = "Playlist",
    YearPublished = 2024,
    NumPages = 416,
    Description = "\r\nA month ago, fifteen-year-old Feline Jagow disappeared, presumed abducted, on her way to school. Her distraught mother asks private investigator Alexander Zorbach, a former police detective, to discover her whereabouts.\r\n\r\nDespite a month having elapsed since her disappearance, Feline's music playlist was changed just a few days ago. Could a seemingly innocuous list of songs contain a hidden clue to where the girl is being held - and how she can be rescued - or is the truth something more sinister?\r\n\r\nSoon, the mystery of the playlist plunges Zorbach into a horrifying nightmare where no one's survival is guaranteed...",
    Publisher = "Bloomsbury Publishing",
    FrontPage = "https://m.media-amazon.com/images/I/91eZ-lrMF-L._AC_UF1000,1000_QL80_.jpg",
    DownloadUrl = "https://www.amazon.com/Playlist-Psychothriller/dp/3426281562",
    AuthorId = 1
});
                context.SaveChanges();

                context.BookGenre.AddRange(
                    new BookGenre { GenreId = 7, BookId = 1 },
 new BookGenre { GenreId = 5, BookId = 1 },
 new BookGenre { GenreId = 7, BookId = 2 },
 new BookGenre { GenreId = 5, BookId = 2 },
 new BookGenre { GenreId = 7, BookId = 3 },
 new BookGenre { GenreId = 5, BookId = 3 },
 new BookGenre { GenreId = 3, BookId = 4 },
 new BookGenre { GenreId = 4, BookId = 4 },
new BookGenre { GenreId = 3, BookId = 5 },
new BookGenre { GenreId = 4, BookId = 5 },
new BookGenre { GenreId = 3, BookId = 6 },
new BookGenre { GenreId = 4, BookId = 6 },
new BookGenre { GenreId = 1, BookId = 7 },
new BookGenre { GenreId = 6, BookId = 7 },
new BookGenre { GenreId = 1, BookId = 8 },
new BookGenre { GenreId = 6, BookId = 8 },
new BookGenre { GenreId = 1, BookId = 9 },
new BookGenre { GenreId = 6, BookId = 9 });
                context.SaveChanges();


                context.Review.AddRange(
    new Review
    {
        BookId = 1,
        AppUser = "User 1",
        Comment = "Love it",
        Rating = 5
    },
    new Review
    {
        BookId = 1,
        AppUser = "User 2",
        Comment = "I did not understand it",
        Rating = 1
    },
    new Review
    {
        BookId = 2,
        AppUser = "User 3",
        Comment = "Boring",
        Rating = 2
    },
    new Review
    {
        BookId = 1,
        AppUser = "User 4",
        Comment = "Well this is stupid",
        Rating = 2
    },
    new Review
    {
        BookId = 3,
        AppUser = "User 5",
        Comment = "No comment",
        Rating = 5
    },
    new Review
    {
        BookId = 3,
        AppUser = "User 6",
        Comment = "Will read 3 more times!",
        Rating = 5
    },
    new Review
    {
        BookId = 5,
        AppUser = "User 5",
        Comment = "Scary",
        Rating = 3
    },
    new Review
    {
        BookId = 4,
        AppUser = "User 7",
        Comment = "what is this",
        Rating = 4
    },
    new Review
    {
        BookId = 6,
        AppUser = "User 9",
        Comment = "Not for me so scary",
        Rating = 3
    },
    new Review
    {
        BookId = 9,
        AppUser = "User 10",
        Comment = "Wicked",
        Rating = 3
    },
    new Review
    {
        BookId = 7,
        AppUser = "User 12",
        Comment = "Wicked",
        Rating = 3
    },
     new Review
     {
         BookId = 8,
         AppUser = "User 69",
         Comment = "Inspirational",
         Rating = 5
     },
    new Review
    {
        BookId = 9,
        AppUser = "User 8",
        Comment = "Na Jovana i e omilena",
        Rating = 5
    }
);
                context.SaveChanges();


            }
        }
    }
}

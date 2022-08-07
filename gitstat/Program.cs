using System;
using System.Linq;
using LibGit2Sharp;

namespace GitStat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Print out the authors and the amount of commits in a GIT repository");
            Console.WriteLine();

            if (args.Length == 0)
            {
                Console.WriteLine("No path to the repository specified");
                Console.WriteLine("Example: gitstat.exe \"c:\\myrepository\"");
                return;
            }
            
            using (var repo = new Repository(args[0]))
            {
                var totalCommitsByAuthor = repo.Commits.GroupBy(t => t.Author.Name, (t, v) => new { AuthorName = t, Count = v.Count() }).ToArray();
                var totalCommits = totalCommitsByAuthor.Sum(t => t.Count);
                foreach (var author in totalCommitsByAuthor)
                {
                    var percentage = author.Count == 0 ? 0 : totalCommits / author.Count * 100;
                    Console.WriteLine($"{author.AuthorName} [{author.Count}] {percentage}%");
                }

                var previousMonth = DateTime.UtcNow.AddMonths(-1);
                var totalCommitsByAuthorLastMonth = repo.Commits.Where(t => t.Author.When > previousMonth).GroupBy(t => t.Author.Name, (t, v) => new { AuthorName = t, Count = v.Count() }).ToArray();
                var totalCommitsLastMonth = totalCommitsByAuthorLastMonth.Sum(t => t.Count);
                foreach (var author in totalCommitsByAuthorLastMonth)
                {
                    var percentage = author.Count == 0 ? 0 : totalCommitsLastMonth / author.Count * 100;
                    Console.WriteLine($"{author.AuthorName} [{author.Count}] {percentage}%");
                }
            }
        }
    }
}

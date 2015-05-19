using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NManga.Models;

namespace NManga.DataAccess
{
    public interface IComicDac
    {
        int GetComicCount();
        IList<Comic> GetAllByPublishDate();
        Comic GetComicById(int id);
        Comic GetComicByOrdinal(int ordinal);
        Comic GetLatestComic();
        void InsertComic(Comic comic);
        void UpdateComic(Comic comic);
        void ReindexComics();
    }

    public class ComicDac : IComicDac
    {
        private readonly ApplicationDbContext _dbContext;

        public ComicDac(ApplicationDbContext dbContext = null)
        {
            _dbContext = dbContext ?? null;
        }

        public int GetComicCount()
        {
            return _dbContext.Comics.Count();
        }

        public Comic GetComicById(int id)
        {
            return _dbContext.Comics.FirstOrDefault(x => x.Id == id);
        }

        public Comic GetComicByOrdinal(int ordinal)
        {
            return _dbContext.Comics.FirstOrDefault(x => x.Ordinal == ordinal);
        }

        public Comic GetLatestComic()
        {
            return _dbContext.Comics.OrderByDescending(x => x.TimePublished).FirstOrDefault();
        }

        public IList<Comic> GetAllByPublishDate()
        {
            return _dbContext.Comics.OrderByDescending(x => x.TimePublished).ToList();
        }

        public void InsertComic(Comic comic)
        {
            _dbContext.Comics.Add(comic);
            _dbContext.SaveChanges();
        }

        public void UpdateComic(Comic comic)
        {
            _dbContext.Entry<Comic>(comic).CurrentValues.SetValues(comic);
            _dbContext.SaveChanges();
        }

        public void ReindexComics()
        {
            var comics = _dbContext.Comics.OrderBy(x => x.TimePublished);
            var ordinal = 1;

            foreach(var comic in comics)
            {
                comic.Ordinal = ordinal;
                _dbContext.Entry(comic).CurrentValues.SetValues(comic);

                ordinal++;
            }

            _dbContext.SaveChanges();
        }
    }
}
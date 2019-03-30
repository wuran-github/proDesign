using models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL
{
    /// <summary>
    /// 视图DAL
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ViewDAL<T> where T:class
    {
        DBDepotEntities db = new DBDepotEntities();
        public ViewDAL()
        {
            //暂定
            db.Configuration.ValidateOnSaveEnabled = false;
        }
        #region 按Lambda表达式查询
        /// <summary>
        /// 普通查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> where)
        {
            var dbSet = db.Set<T>();
            return dbSet.Where(where).AsNoTracking().ToList();
        }

        #endregion
        #region 关联查询
        /// <summary>
        /// 关联查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="include">要关联的表</param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> where, params Expression<Func<T, dynamic>>[] include)
        {
            var dbSet = db.Set<T>();
            var t = dbSet.Where(where);
            foreach (var i in include)
            {
                t = t.Include(i);
            }
            return t.AsNoTracking().ToList();
        }

        #endregion
        #region 返回查询的条数
        /// <summary>
        /// 返回查询的条数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetCount(Expression<Func<T, bool>> where)
        {
            return db.Set<T>().Where(where).AsNoTracking().Count();
        }
        #endregion
        #region 分页查询，并根据字段排序
        /// <summary>
        /// 升序分页查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<T> GetPageList<TKey>(int pageIndex, int PageSize, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy)
        {
            return db.Set<T>().Where(where).OrderBy(orderBy).AsNoTracking().Skip(pageIndex * PageSize).Take(PageSize).ToList();
        }
        /// <summary>
        /// 升序分页关联查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public List<T> GetPageList<TKey>(int pageIndex, int PageSize, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, params Expression<Func<T, dynamic>>[] include)
        {
            var t = db.Set<T>().Where(where);
            foreach (var i in include)
            {
                t = t.Include(i);
            }
            return t.OrderBy(orderBy).AsNoTracking().Skip(pageIndex * PageSize).Take(PageSize).ToList();
        }
        /// <summary>
        /// 降序分页查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<T> GetPageListDesc<TKey>(int pageIndex, int PageSize, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy)
        {
            return db.Set<T>().Where(where).AsNoTracking().OrderByDescending(orderBy).Skip(pageIndex * PageSize).Take(PageSize).ToList();
        }
        /// <summary>
        /// 降序分页关联查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public List<T> GetPageListDesc<TKey>(int pageIndex, int PageSize, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, params Expression<Func<T, dynamic>>[] include)
        {
            var t = db.Set<T>().Where(where);
            foreach (var i in include)
            {
                t = t.Include(i);
            }
            return t.OrderByDescending(orderBy).AsNoTracking().Skip(pageIndex * PageSize).Take(PageSize).ToList();
        }
        #endregion
    }
}

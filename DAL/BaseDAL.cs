using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models;
using System.Reflection;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Data.Entity;
namespace DAL
{
    public class BaseDAL<T> where T : class,new()
    {
        DBDepotEntities db = new DBDepotEntities();
        static DBDepotEntities singleDB = SingleContext.db;
        public BaseDAL()
        {
            //暂定
            db.Configuration.ValidateOnSaveEnabled = false;
        }
        #region 增加实体
        /// <summary>
        /// 增加单个实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int Add(T t)
        {
            try
            {
                db.Set<T>().Add(t);
                return db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
        #region 增加多个实体
        /// <summary>
        /// 增加多个实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int AddRange(List<T> t)
        {
            try
            {
                db.Set<T>().AddRange(t);
                return db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
        #region 删除实体
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int Delete(T t)
        {
            try
            {
                db.Set<T>().Remove(t);
                return db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        #region 批量删除
        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public int DeleteRange(List<T> ts)
        {
            try
            {
                db.Set<T>().RemoveRange(ts);
                return db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region 修改实体
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int Modify(T t)
        {
            try
            {
                DbEntityEntry<T> entry = db.Entry<T>(t);
                entry.State = EntityState.Modified;
                return db.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// 修改多个实体
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int Modify(List<T> list)
        {
            try
            {
                foreach (var t in list)
                {
                    DbEntityEntry<T> entry = db.Entry<T>(t);
                    entry.State = EntityState.Modified;
                }
                return db.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion
        #region 按Lambda表达式查询
        /// <summary>
        /// 普通查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> where)
        {
            var dbSet = db.Set<T>();
            return dbSet.Where(where).ToList();
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
            var t = db.Set<T>().Where(where);
            foreach (var i in include)
            {
                t = t.Include(i);
            }
            return t.ToList();
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
            return db.Set<T>().Where(where).Count();
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
            return db.Set<T>().Where(where).OrderBy(orderBy).Skip(pageIndex * PageSize).Take(PageSize).ToList();
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
            return t.OrderBy(orderBy).Skip(pageIndex * PageSize).Take(PageSize).ToList();
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
            return db.Set<T>().Where(where).OrderByDescending(orderBy).Skip(pageIndex * PageSize).Take(PageSize).ToList();
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
            return t.OrderByDescending(orderBy).Skip(pageIndex * PageSize).Take(PageSize).ToList();
        }
        #endregion
        /// <summary>
        /// 把dbcontext对象销毁，为了避免各种冲突
        /// </summary>
        public void Dispose()
        {
            db.Dispose();
        }
        #region 单例数据上下文操作
        /// <summary>
        /// 单例查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<T> SingleContextGetList(Expression<Func<T, bool>> where)
        {
            var dbSet = singleDB.Set<T>();
            return dbSet.Where(where).ToList();
        }
        /// <summary>
        /// 单例修改
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int SingleContextModify(T t)
        {
            try
            {
                DbEntityEntry<T> entry = singleDB.Entry<T>(t);
                singleDB.Set<T>().Attach(t);
                entry.State = EntityState.Modified;
                return singleDB.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// 单例删除实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int SingleContextDelete(T t)
        {
            try
            {
                singleDB.Set<T>().Remove(t);
                return singleDB.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 单例数据上下文增加多个实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int SingleContextAddRange(List<T> t)
        {
            try
            {
                singleDB.Set<T>().AddRange(t);
                return singleDB.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 单例数据上下文增加实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int SingleContextAdd(T t)
        {
            try
            {
                singleDB.Set<T>().Add(t);
                return singleDB.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 单例关联查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public List<T> SingleContextGetList(Expression<Func<T, bool>> where, params Expression<Func<T, dynamic>>[] include)
        {
            var t = singleDB.Set<T>().Where(where);
            foreach (var i in include)
            {
                t = t.Include(i);
            }
            return t.ToList();
        }
        #endregion
    }
}

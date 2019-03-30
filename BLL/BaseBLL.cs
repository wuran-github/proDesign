using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public abstract class BaseBLL<T> where T : class,new()
    {
        protected BaseDAL<T> dal = null;
        protected abstract void SetDAL();

        public BaseBLL()
        {
            SetDAL();
        }
        #region 增加实体
        /// <summary>
        /// 增加单个实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int Add(T t)
        {
            return dal.Add(t);
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
            return dal.AddRange(t);
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
            return dal.Delete(t);
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
            return dal.DeleteRange(ts);

        }
        #endregion
        #region 修改实体
        public int Modify(T t)
        {
            return dal.Modify(t);
        }
        /// <summary>
        /// 修改多个实体
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int Modify(List<T> list)
        {
            return dal.Modify(list);
        }
        #endregion
        #region 按Lambda表达式查询
        /// <summary>
        /// 按Lambda表达式查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> where)
        {
            return dal.GetList(where);
        }
        #endregion
        #region
        public List<T> GetList(Expression<Func<T, bool>> where, params Expression<Func<T, dynamic>>[] include)
        {
            return dal.GetList(where, include);
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
            return dal.GetCount(where);
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
            return dal.GetPageList(pageIndex, PageSize, where, orderBy);
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
            return dal.GetPageList(pageIndex, PageSize, where, orderBy, include);
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
            return dal.GetPageListDesc(pageIndex, PageSize, where, orderBy);
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
            return dal.GetPageListDesc(pageIndex, PageSize, where, orderBy, include);
        }
        #endregion
        #region 单例数据上下文操作
        /// <summary>
        /// 单例查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<T> SingleContextGetList(Expression<Func<T, bool>> where)
        {
            return dal.SingleContextGetList(where);
        }
        /// <summary>
        /// 单例修改
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int SingleContextModify(T t)
        {
            return dal.SingleContextModify(t);
        }
        /// <summary>
        /// 单例删除实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int SingleContextDelete(T t)
        {
            return dal.SingleContextDelete(t);
        }
        /// <summary>
        /// 单例数据上下文增加多个实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int SingleContextAddRange(List<T> t)
        {
            return dal.SingleContextAddRange(t);
        }

        /// <summary>
        /// 单例数据上下文增加实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int SingleContextAdd(T t)
        {
            return dal.SingleContextAdd(t);
        }
        /// <summary>
        /// 单例关联查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public List<T> SingleContextGetList(Expression<Func<T, bool>> where, params Expression<Func<T, dynamic>>[] include)
        {
            return dal.SingleContextGetList(where, include);
        }
        #endregion
        /// <summary>
        /// 销毁数据上下文对象
        /// </summary>
        public void Dispose()
        {
            dal.Dispose();
        }

    }
}

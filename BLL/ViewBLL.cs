using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public abstract class ViewBLL<T> where T:class
    {
       protected DAL.ViewDAL<T> dal=null;
       protected abstract void SetDAL();
       public ViewBLL()
        {
            SetDAL();
        }
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
    }
}

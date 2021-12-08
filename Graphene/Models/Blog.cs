using GrapheneCore.Models;

namespace Graphene.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Blog : Model
    {
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Post> Posts { get; } = new();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public dynamic Get(string path)
        {
            return QueryableOf(this).Where(m => m.Id == Id);
        }
        /// <summary>
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<dynamic> QueryableOf<T>() where T : class
        {
            var Object = Activator.CreateInstance(typeof(T), new object[] { });
            return (new List<T>() { ((T)Object) }).AsQueryable();
        }
        /// <summary>
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<dynamic> DynamicQueryableOf<T>(T instance) where T : class
        {
            return (new List<T>() { instance }).AsQueryable();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> QueryableOf<T>(T Entity)
        {
            return (new List<T>() { Entity }).AsQueryable();
        }
    }
}

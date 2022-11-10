using Graphene.Entities.Interfaces;
using Graphene.Http.Exceptions;
using Graphene.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Http.Binders
{
    /// <summary>
    /// Attribute that binds with FetchEntityModelBinder
    /// (Sugar syntax Attribute to use in the controllers insteadof [Modelbinder(typeof(FetchEntityModelBinder))])
    /// </summary>
    public class FindEntity : ModelBinderAttribute
    {
        /// <summary>
        /// Initialize the modelbinderAttribute with the FetchEntityModelBinder
        /// </summary>
        public FindEntity() : base(typeof(FetchEntityModelBinder))
        {
            // Nothing for now.
        }
    }

    /// <summary>
    /// Finds and fetch the requested https://api.foo.bar/{entity}?loads[]=NavegationProperty
    /// and it load its requested NavegationProperties.
    /// </summary>
    public class FetchEntityModelBinder : IModelBinder
    {
        /// <summary>
        /// Current Scoped IEntityContext instance
        /// </summary>
        private readonly IEntityContext _context;


        /// <summary>
        /// Initialize the Binder with the current Scoped IEntityContext instance
        /// </summary>
        /// <param name="context">Current Scoped IEntityContext instance</param>
        public FetchEntityModelBinder(IEntityContext context)
        {
            // Assing the IEntityContext instance for latter use.
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="StatusCodeException"></exception>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Must have the context
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));
            // Get the loas Query param value to Include in the DbSet query
            string[] load = bindingContext.ValueProvider.GetValue("load[]").Values.ToArray();
            // Initialize iinstance
            IEntity? instance = null;
            // Try get the requested resource, and load the requested eF NavigationProperty
            try { instance = _context.FindInstance(load); }
            // If fails is probably becouse the navigation property doesnt exists so BAD Request.
            catch (System.InvalidOperationException e) { throw new StatusCodeException(new BadRequestObjectResult(new { error = e.Message })); }
            // If returns null throw 404 response, the exception StatusCode Exception  Filter will take care.
            if (instance == null) throw new StatusCodeException(new NotFoundResult());
            // If returns an instance assign that as the model binder value
            bindingContext.Result = ModelBindingResult.Success(instance);
            // compleate task.... should I compleate the task before throwing an error?
            return Task.CompletedTask;
        }
    }
}
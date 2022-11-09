using Graphene.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Http
{}
public class EntityModelBinder : IModelBinder
{
    private readonly IEntityContext _context;

    public EntityModelBinder(IEntityContext context)
    {
        _context = context;
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));
        bindingContext.Result = ModelBindingResult.Success(_context.FindInstance());
        return Task.CompletedTask;
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SolForms.Models;
using SolForms.Models.Questions;
using System.Net.Http;

namespace SolFormsApi.Client
{
    public interface ISolFormsClient
    {
        Task<TEntity?> Get<TEntity>(Guid id);
        Task<TEntity?> GetAll<TEntity>();
        Task Post<TEntity>(TEntity data);
        Task<TResponse?> Post<TResponse, TEntity>(TEntity data);
        Task Put<TEntity>(TEntity data);
        Task<TResponse?> Put<TResponse, TEntity>(TEntity data);
        Task Delete<T>(Guid id);
    }
}

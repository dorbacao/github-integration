using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using Octokit.Internal;

namespace github_integration
{
    internal static class Github
    {
        public static async Task CreateRepository()
        {

            //Aqui estou usando a autenticação baseada em PAT (Personal Access Token), mas você pode usar qualquer outro tipo de autenticação, há vários tipos de autenticação suportados (consulte a captura de tela abaixo)
            var tokenAuth = new Credentials("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");
            var client = new GitHubClient(new ProductHeaderValue("ingresso-app"));
            client.Credentials = tokenAuth;

            Console.WriteLine($"Obtendo o total de repositórios");
            //Pesquisa de todos os repositórios para determinada conta de usuário
            var allRepository = await client.Repository.GetAllForCurrent();

            Console.WriteLine($"Total Repositorios encontrados: {allRepository.Count}");

            var myProjectName = "my-repository-template";

            var curentRepo = allRepository.FirstOrDefault(a => a.Name == myProjectName);

            if (curentRepo != null)
            {
                Console.WriteLine($"Repositório já existe, excluindo...");
                await client.Repository.Delete(curentRepo.Id);
                Console.WriteLine($"Repositório de nome {myProjectName} excluido com sucesso.");
            }

            var newRepo = new NewRepository(myProjectName)
            {
                AutoInit = true,
                Description = "Este é o repositório do código-fonte",
                HasIssues = false,
                HasWiki = true,
                Private = false
            };

            Console.WriteLine($"Criando repositório de nome '{myProjectName}'");

            var repositoryResponse = await client.Repository.Create(newRepo);

            Console.WriteLine(repositoryResponse.FullName);

            Console.WriteLine($"Repositórios criados com sucesso.");

            var content = File.ReadAllText(@"./README.md");
            var createFileRequest = new CreateFileRequest("ComoCriarUmPat.md", content);
            var fileCreateResponse = client.Repository.Content.CreateFile(repositoryResponse.Id, @"ComoCriarUmPat.md", createFileRequest);

            allRepository = await client.Repository.GetAllForCurrent();

            Console.WriteLine($"{allRepository.Count} repositórios encontrados");

            Console.WriteLine($"Url do novo repositório: {repositoryResponse.Url}");
        }
    }
}

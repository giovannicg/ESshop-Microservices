namespace Catalog.API.Products.CreateProduct
{
	public record CreateProductRequest(
		string Name, 
		List<string> Category, 
		string Description, 
		string ImageFile, 
		decimal Price
		);

	public record CreateProductResponse(Guid Id);

	/// <summary>
	/// Represents the endpoint for creating a new product.
	/// </summary>
	public class CreateProductEndpoint : ICarterModule
	{
		/// <summary>
		/// Adds routes for the create product endpoint.
		/// </summary>
		/// <param name="app">The endpoint route builder.</param>
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapPost("/products", 
				async (CreateProductRequest request, ISender sender) =>
			{
				/*

				 La línea de código var command = request.Adapt<CreateProductCommand>(); 
				toma el objeto request de tipo CreateProductRequest y lo adapta o mapea a un objeto CreateProductCommand.
				Esto permite que los datos del objeto request se asignen a las propiedades correspondientes del objeto command, 
				de modo que se puedan utilizar para realizar la operación de creación del producto.
				 */
				var command = request.Adapt<CreateProductCommand>();

				var result = await sender.Send(command);

				var response = result.Adapt<CreateProductResponse>();

				return Results.Created($"/products/{response.Id}", response);
			})
			.WithName("CreateProduct")
			.Produces<CreateProductResponse>(StatusCodes.Status201Created)
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.WithSummary("Create a new product")
			.WithDescription("Create a new product in the catalog");
		}
	}
}

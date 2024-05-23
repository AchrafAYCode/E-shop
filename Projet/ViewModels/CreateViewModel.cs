using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Projet.ViewModels
{
	public class CreateViewModel
	{
		public int ProductId { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 5)]
		public string Description { get; set; }

		[Required]
		[Display(Name = "Prix en dinar :")]
		public float Price { get; set; }

		[Required]
		[Display(Name = "Quantité en unité :")]
		public int StockQuantity { get; set; }

		[Required]
		[Display(Name = "Image :")]
		public IFormFile ImagePath { get; set; }

		[Required]
		[Display(Name = "Catégorie :")]
		public int CategoryId { get; set; }
	}
}

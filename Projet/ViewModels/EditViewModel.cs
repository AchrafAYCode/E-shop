namespace Projet.ViewModels
{
	public class EditViewModel : CreateViewModel
    {
        public new int ProductId { get; set; } 
        public string ExistingImagePath { get; set; }
        public int CategoryId { get; set; }
    }
}

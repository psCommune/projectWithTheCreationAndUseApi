namespace SuperBank.Domain
{
    public class Category
    {
        public string Name { get; set; } = string.Empty;
        public string CategoryCode { get; set; }
    }
    public class Manager
    {
        public int Number { get; set; } // номер окна

        public List<Category> QuestionCategories { get; set; } =
            new List<Category>(); // категории вопросов, которые может принять
    }
    public class Question
    {
        public int Number { get; set; } // порядковый номер

        public Category Category { get; set; } = null!; // категория

        public Manager? Manager { get; set; } // окно

        public string QuestionCode => $"{Category.CategoryCode}-{Number}"; // Б-151

    }
    public interface IBankQueue
    {
        List<Category> Categories { get; }
        Question Register(Category questionCategory);
        List<Question> Questions { get; }
        void CloseQuestion(Question question);
        Question? CurrentQuestion(int managerNumber);
    }

}
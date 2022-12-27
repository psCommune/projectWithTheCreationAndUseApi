using SuperBank.Domain;

namespace SuperBank.SuperBankQueue
{
    public class SuperBankQueue : IBankQueue
    {
        private readonly ISuperBankBuilder builder;
        private readonly List<Category> categories;
        private readonly List<Manager> managers;
        private readonly List<Question> questions = new List<Question>();
        private readonly Dictionary<Category, int> counter = new Dictionary<Category, int>();
        public List<Category> Categories => categories;

        public List<Question> Questions => questions;
        public SuperBankQueue(ISuperBankBuilder builder)
        {
            this.builder = builder;
            categories = builder.GetCategories();
            managers = builder.GetManagers();
            foreach (Category category in categories)
            {
                counter.Add(category, 1);
            }
        }
        public Question? CurrentQuestion(int managerNumber)
        {
            var manager = managers.FirstOrDefault(m => m.Number == managerNumber);
            if (manager == null)
            {
                throw new ArgumentException("Invalid manager number");
            }

            return questions.FirstOrDefault(q => q.Manager == manager);
        }
        public Question Register(Category questionCategory)
        {
            // если категория существует
            if (!categories.Contains(questionCategory))
            {
                throw new ArgumentException("Invalid category!");
            }

            // находим свободного менеджера, отвечающего
            // за данную категорию
            var freeManager = findFreeManager(questionCategory);

            // формируем новый вопрос 
            var question = new Question
            {
                Category = questionCategory,
                Manager = freeManager,
                // увеличиваем внутренний счетчик
                // после Б-100 должно быть Б-101
                Number = counter[questionCategory]++
            };
            // добавляем вопрос в очередь
            questions.Add(question);
            // возвращаем сформированный вопрос
            return question;
        }
        private Manager? findFreeManager(Category category)
        {
            return managers
                .Where(m => questions.All(q => q.Manager != m))
                .FirstOrDefault(m => m.QuestionCategories.Contains(category));
        }
        public void CloseQuestion(Question question)
        {
            var category = question.Category;
            // удаляем из очереди
            questions.Remove(question);

            // находим свободный вопрос (не обслуживаемый в данный момент)
            var next = questions.FirstOrDefault(q =>
                q.Category == category && q.Manager == null);

            // назначаем свободного менеджера по этой же категории
            if (next != null)
            {
                next.Manager = findFreeManager(category);
            }
        }

       
    }
    public interface ISuperBankBuilder
    {
        List<Category> GetCategories();
        List<Manager> GetManagers();
    }
    public class DefaultSuperBankBuilder : ISuperBankBuilder
    {

        private List<Category> categories;
        private List<Manager> managers;

        public DefaultSuperBankBuilder()
        {
            categories = new List<Category>
        {
            new Category { CategoryCode = "Б", Name = "Банковские карточки" },
            new Category { CategoryCode = "П", Name = "Платежи и переводы" },
            new Category { CategoryCode = "К", Name = "Кредит и ипотека" },
            new Category { CategoryCode = "В", Name = "Обмен валют" },
            new Category { CategoryCode = "М", Name = "Драгоценные металлы" },
            new Category { CategoryCode = "Р", Name = "Разное" }
        };

            var window1 = new Manager
            {
                Number = 1,
                QuestionCategories = new List<Category>
            {
                categories[0], categories[1], categories[2]
            }
            };

            var window2 = new Manager
            {
                Number = 2,
                QuestionCategories = new List<Category>
            {
                categories[0], categories[1], categories[2]
            }
            };

            var window3 = new Manager
            {
                Number = 3,
                QuestionCategories = new List<Category>
            {
                categories[0], categories[1], categories[2]
            }
            };

            var window4 = new Manager
            {
                Number = 4,
                QuestionCategories = new List<Category>
            {
                categories[3], categories[4]
            }
            };

            var window5 = new Manager
            {
                Number = 5,
                QuestionCategories = new List<Category>
            {
                categories[3], categories[4]
            }
            };

            var window6 = new Manager
            {
                Number = 6,
                QuestionCategories = new List<Category>
            {
                categories[5]
            }
            };

            managers = new List<Manager>
        {
            window1, window2, window3, window4, window5, window6
        };

        }

        public List<Category> GetCategories() => categories;

        public List<Manager> GetManagers() => managers;

       

        
    }
}
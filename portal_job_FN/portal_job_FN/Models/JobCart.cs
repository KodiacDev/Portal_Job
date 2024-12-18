namespace portal_job_FN.Models
{
    public class JobCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.PostJobId == item.PostJobId);
                Items.Add(item);
        }
        public void RemoveItem(int PostJobId)
        {
            Items.RemoveAll(i => i.PostJobId == PostJobId);
        }
    }
}

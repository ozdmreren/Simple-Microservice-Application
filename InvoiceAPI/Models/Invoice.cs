namespace InvoiceAPI.Models
{
    public static class Invoice
    {

        public static string Write(dynamic customer, dynamic address)
        {
            string message = $@"
                            -----Belgrad-----
                
            Belgrad Kullanarak Alışveriş Yaptığınız İçin Teşekkürler

            Müşteri Adres Bilgileri:
            Ülke: {address.Country}
            Şehir: {address.City}
            Semt: {address.District}
            Bina No: {address.BlockNo}
            Kapı No: {address.DoorNo}

            Müşteri Bilgileri:
            Müşteri Adı: {customer.Firstname}
            Müşteri Soyadı: {customer.Surname}
            Müşteri Telefon Numarası: {customer.PhoneNumber}

            Ürün Bilgileri
            Alınan Ürünler: .....

            Toplam Tutar: 4568₺
            ";

            return message;
        }
    }
}
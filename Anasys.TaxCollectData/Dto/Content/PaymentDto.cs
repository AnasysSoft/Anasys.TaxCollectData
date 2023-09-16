namespace Anasys.TaxCollectData.Dto.Content;

/// <summary>
/// Invoice payment
/// </summary>
/// <param name="Iinn">شماره سوییچ پرداخت</param>
/// <param name="Acn">شماره پذیرنده فروشگاهی</param>
/// <param name="Trmn">شماره پایانه</param>
/// <param name="Trn">شماره پیگیری</param>
/// <param name="Pcn">شماره کارت پرداخت کننده صورتحساب</param>
/// <param name="Pid">شماره/شناسه ملی/کد فراگیر اتباع غیر ایرانی پرداخت کننده صورتحساب</param>
/// <param name="Pdt">تاریخ و زمان پرداخت صورتحساب</param>
public record PaymentDto(string Iinn, 
    string Acn, 
    string Trmn, 
    string Trn, 
    string Pcn, 
    string Pid, 
    long Pdt);
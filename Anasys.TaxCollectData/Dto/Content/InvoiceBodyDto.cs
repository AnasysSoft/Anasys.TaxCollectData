namespace Anasys.TaxCollectData.Dto.Content;

/// <summary>
/// Invoice item
/// </summary>
/// <param name="Sstid">شناسه کالا/خدمت</param>
/// <param name="Sstt">شرح کالا/خدمت</param>
/// <param name="Mu">واحد اندازه گیری</param>
/// <param name="Am">تعداد/مقدار</param>
/// <param name="Fee">مبلغ واحد</param>
/// <param name="Cfee">میزان ارز</param>
/// <param name="Cut">نوع ارز</param>
/// <param name="Exr">نرخ برابری ارز با ریال</param>
/// <param name="Prdis">مبلغ قبل از تخفیف</param>
/// <param name="Dis">مبلغ تخفیف</param>
/// <param name="Adis">مبلغ بعد از تخفیف</param>
/// <param name="Vra">نرخ مالیات بر ارزش افزوده</param>
/// <param name="Vam">مبلغ مالیات بر ارزش افزوده</param>
/// <param name="Odt">موضوع سایر مالیات و عوارض</param>
/// <param name="Odr">نرخ سایر مالیات و عوارض</param>
/// <param name="Odam">مبلغ سایر مالیات و عوارض</param>
/// <param name="Olt">موضوع سایر وجوه قانونی</param>
/// <param name="Olr">نرخ سایر وجوه قانونی</param>
/// <param name="Olam">مبلغ سایر وجوه قانونی</param>
/// <param name="Consfee">اجرت ساخت</param>
/// <param name="Spro">سود فروشنده</param>
/// <param name="Bros">حق العمل</param>
/// <param name="Tcpbs">جمع کل اجرت، حق العمل و سود</param>
/// <param name="Cop">سهم نقدی از پرداخت</param>
/// <param name="Bsrn">شناسه یکتای ثبت قرارداد حق العملکاری</param>
/// <param name="Vop">سهم ارزش افزوده از پرداخت</param>
/// <param name="Tsstam"> مبلغ کل کالا/خدمت</param>
public record InvoiceBodyDto(string Sstid, 
    string Sstt, 
    int Mu, 
    double Am, 
    decimal Fee, 
    decimal Cfee, 
    string Cut, 
    string Exr, 
    decimal Prdis, 
    decimal Dis, 
    decimal Adis, 
    decimal Vra, 
    decimal Vam, 
    string Odt, 
    decimal Odr, 
    decimal Odam, 
    string Olt, 
    decimal Olr, 
    decimal Olam, 
    decimal Consfee, 
    decimal Spro, 
    decimal Bros, 
    decimal Tcpbs, 
    decimal Cop, 
    string Bsrn, 
    string Vop, 
    decimal Tsstam);
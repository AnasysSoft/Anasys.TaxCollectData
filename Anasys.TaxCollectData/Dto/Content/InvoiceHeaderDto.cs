namespace Anasys.TaxCollectData.Dto.Content;

/// <summary>
/// Invoice header
/// </summary>
/// <param name="Indati2m">تاریخ و زمان ایجاد صورتحساب)میلادی(</param>
/// <param name="Indatim">تاریخ و زمان صدور صورتحساب)میلادی(</param>
/// <param name="Inty">نوع صورتحساب</param>
/// <param name="Ft">نوع پرواز</param>
/// <param name="Inno">سریال صورتحساب</param>
/// <param name="Irtaxid">شماره منحصر به فرد مالیاتی صورتحساب مرجع</param>
/// <param name="Scln">شماره پروانه گمرکی فروشنده</param>
/// <param name="Setm">روش تسویه</param>
/// <param name="Tins">شماره اقتصادی فروشنده</param>
/// <param name="Cap">مبلغ پرداختی نقدی</param>
/// <param name="Bid">شماره/شناسه ملی/شناسه مشارکت مدنی/کد فراگیر خریدار</param>
/// <param name="Insp">مبلغ پرداختی نسیه</param>
/// <param name="Tvop">مجموع سهم مالیات بر ارزش افزوده از پرداخت</param>
/// <param name="Bpc">کد پستی خریدار</param>
/// <param name="Tax17">مالیات موضوع ماده 17</param>
/// <param name="Taxid">شماره منحصر به فرد مالیاتی</param>
/// <param name="Inp">الگوی صورتحساب</param>
/// <param name="Scc">کد گمرک محل اظهار</param>
/// <param name="Ins">موضوع صورتحساب</param>
/// <param name="Billid">شماره اشتراک/ شناسه قبض بهره بردار</param>
/// <param name="Tprdis">مجموع مبلغ قبل از کسر تخفیف</param>
/// <param name="Tdis">مجموع تخفیفات</param>
/// <param name="Tadis">مجموع مبلغ پس از کسر تخفیف</param>
/// <param name="Tvam">مجموع مالیات بر ارزش افزوده</param>
/// <param name="Todam">مجموع سایر مالیات، عوارض و وجوه قانونی</param>
/// <param name="Tbill">مجموع صورتحساب</param>
/// <param name="Tob">نوع شخص خریدار</param>
/// <param name="Tinb">شماره اقتصادی خریدار</param>
/// <param name="Sbc">کد شعبه فروشنده</param>
/// <param name="Bbc">کد شعبه خریدار</param>
/// <param name="Bpn">شماره گذرنامه خریدار</param>
/// <param name="Crn">شناسه یکتای ثبت قرارداد فروشنده</param>
public record InvoiceHeaderDto(long Indati2m, 
    long Indatim, 
    int Inty, 
    int Ft, 
    long Inno, 
    string Irtaxid, 
    long Scln, 
    int Setm, 
    string Tins, 
    decimal Cap, 
    string Bid, 
    decimal Insp, 
    decimal Tvop, 
    string Bpc, 
    decimal Tax17, 
    string Taxid, 
    int Inp, 
    string Scc, 
    int Ins, 
    string Billid, 
    decimal Tprdis, 
    decimal Tdis, 
    decimal Tadis, 
    decimal Tvam, 
    decimal Todam, 
    decimal Tbill, 
    int Tob, 
    string Tinb, 
    string Sbc, 
    string Bbc, 
    string Bpn, 
    int Crn);
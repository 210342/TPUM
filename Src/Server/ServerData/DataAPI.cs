/* Warstwa Dane
 * Jako repozytorium danych procesowych należy wykorzystać model obiektowy w pamięci operacyjnej.
 * Początkowy stan procesu biznesowego należy odtworzyć z wygenerowanych/odczytanych danych,
 * z zastrzeżeniami opisanymi w rozdziale Wytyczne do realizacji.
 * Warstwa powinna udostępniać publiczne abstrakcyjne API, a ukrywać szczegóły implementacji.
 */
namespace TPUM.Server.Data
{
    public abstract class DataAPI
    {

        public string Property1 { get; set; }
        public abstract void AbstractMethod();

    }
}
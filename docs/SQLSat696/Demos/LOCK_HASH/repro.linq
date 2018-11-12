<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

int numThreads = 500;
ThreadPool.SetMinThreads(Environment.ProcessorCount * 25, Environment.ProcessorCount * 8);

Parallel.For(0, numThreads, i =>
{
try
{
 using (var con = new SqlConnection("Data Source=.\\sql2017;Initial Catalog=lockhashrepro;Integrated Security=false;user id=nonadmin;password=somepassword;Max Pool Size=5000;Connection Timeout=300;Encrypt=false;TransparentNetworkIPResolution=false;"))
 {
  con.Open();
  var cmd = new SqlCommand();
  cmd.Connection = con;
  cmd.CommandText = "interop";
  cmd.CommandType = CommandType.StoredProcedure;
  
  int cnt = 0;
  while (cnt < 1000000)
  {
  	cmd.ExecuteNonQuery();
	cnt++;
	}
 } 
 }
 catch(Exception)
 {
 	//Console.WriteLine($"Error with {endpoint}");
 }
}
);

/*
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
SOFTWARE. 

This sample code is not supported under any Microsoft standard support program or service.  
The entire risk arising out of the use or performance of the sample scripts and documentation remains with you.  
In no event shall Microsoft, its authors, or anyone else involved in the creation, production, or delivery of the scripts 
be liable for any damages whatsoever (including, without limitation, damages for loss of business profits, 
business interruption, loss of business information, or other pecuniary loss) arising out of the use of or inability 
to use the sample scripts or documentation, even if Microsoft has been advised of the possibility of such damages. 
*/

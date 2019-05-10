<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

int numThreads = 500;
ThreadPool.SetMinThreads(Environment.ProcessorCount * 25, Environment.ProcessorCount * 8);

// Number of threads by default is 128 below. Please DECREASE or (rarely) increase as per your hardware setup
Parallel.For(0, 128, i =>
{
 using (var con = new SqlConnection(@"Data Source=somesql;Initial Catalog=pfscontention;Integrated Security=false;user id=nonadmin;password=somepassword;Max Pool Size=500"))
 {
  string lob = new string('~', 500000);	// 500KB string
  con.Open();
  
  // store a value
  var cmdSet = new SqlCommand();
  cmdSet.Connection = con;
  cmdSet.CommandText = "SetCacheItem";
  cmdSet.CommandType = CommandType.StoredProcedure;
  cmdSet.Parameters.AddWithValue("@Key", Thread.CurrentThread.ManagedThreadId.ToString() + i.ToString());
  cmdSet.Parameters.AddWithValue("@Value", System.Text.Encoding.Unicode.GetBytes(lob));
  cmdSet.Parameters.AddWithValue("@Expiration", DateTime.Now.AddMinutes(5));
  cmdSet.ExecuteNonQuery();
  
  // setup the SqlCommand to get the value
  var cmdGet = new SqlCommand();
  cmdGet.Connection = con;
  cmdGet.CommandText = "GetCacheItem";
  cmdGet.CommandType = CommandType.StoredProcedure;
  cmdGet.Parameters.AddWithValue("@Key", Thread.CurrentThread.ManagedThreadId.ToString() + i.ToString());
  SqlParameter outval = new SqlParameter("@Value", SqlDbType.VarBinary)
   { 
      Direction = ParameterDirection.Output,
	  Size = -1
   };
   cmdGet.Parameters.Add(outval);
  
  int cnt = 0;
  long localsum = 0;
  while (cnt < 100000)
  {
  	cmdGet.ExecuteNonQuery();
	cnt++;
   }
 } 
});
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
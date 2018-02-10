<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

Parallel.For<SqlConnection>(0, 500, 
() =>
{
    var con = new SqlConnection(@"Data Source=.\SQL2017;Initial Catalog=tempdb;Integrated Security=true;Connection Timeout=60");
	con.Open();
	
	return con;
},
(counter, loopState, con) =>
{
	Console.WriteLine(counter);

	var cmd = new SqlCommand();
	cmd.Connection = con;
	cmd.CommandText = @"exec sp_execute_external_script @language = N'R', @script = N'i <- 16';";
	cmd.CommandType = CommandType.Text;
	
	cmd.ExecuteNonQuery();
	
	return con;
}
,(con) => { 
	con.Close();
	con.Dispose();
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

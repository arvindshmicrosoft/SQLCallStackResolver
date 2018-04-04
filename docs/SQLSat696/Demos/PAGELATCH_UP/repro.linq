<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

// Number of threads by default is 128 below. Please DECREASE or (rarely) increase as per your hardware setup
Parallel.For(0, 128, i =>
{
 using (var con = new SqlConnection("Data Source=.;Initial Catalog=pfscontention;Integrated Security=false;user id=nonadmin;password=somepassword;Max Pool Size=500"))
 {
  string lob = new string('~', 3000000);	// 60KB string
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
	
	//localsum += System.Text.Encoding.Unicode.GetString((byte[])outval.Value).Length;
	}
 } 
});
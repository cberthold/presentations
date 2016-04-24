using Raven.Abstractions;
using Raven.Database.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using Raven.Database.Linq.PrivateExtensions;
using Lucene.Net.Documents;
using System.Globalization;
using System.Text.RegularExpressions;
using Raven.Database.Indexing;

public class Index_Auto_DocumentDatas_ByAggregateIdAndVersionAndVersion_RangeSortByAggregateIdVersion : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_DocumentDatas_ByAggregateIdAndVersionAndVersion_RangeSortByAggregateIdVersion()
	{
		this.ViewText = @"from doc in docs.DocumentDatas
select new {
	Version = doc.Version,
	AggregateId = doc.AggregateId
}";
		this.ForEntityNames.Add("DocumentDatas");
		this.AddMapDefinition(docs => 
			from doc in ((IEnumerable<dynamic>)docs)
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "DocumentDatas", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				Version = doc.Version,
				AggregateId = doc.AggregateId,
				__document_id = doc.__document_id
			});
		this.AddField("Version");
		this.AddField("AggregateId");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("Version");
		this.AddQueryParameterForMap("AggregateId");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("Version");
		this.AddQueryParameterForReduce("AggregateId");
		this.AddQueryParameterForReduce("__document_id");
	}
}

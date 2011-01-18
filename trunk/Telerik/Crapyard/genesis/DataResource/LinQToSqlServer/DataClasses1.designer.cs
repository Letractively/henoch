﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.21006.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataResource.LinQToSqlServer
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="RetPok")]
	public partial class DataClasses1DataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertBronBestand(BronBestand instance);
    partial void UpdateBronBestand(BronBestand instance);
    partial void DeleteBronBestand(BronBestand instance);
    partial void InsertProject(Project instance);
    partial void UpdateProject(Project instance);
    partial void DeleteProject(Project instance);
    partial void InsertGegevensSoort(GegevensSoort instance);
    partial void UpdateGegevensSoort(GegevensSoort instance);
    partial void DeleteGegevensSoort(GegevensSoort instance);
    partial void InsertImplicLocatie(ImplicLocatie instance);
    partial void UpdateImplicLocatie(ImplicLocatie instance);
    partial void DeleteImplicLocatie(ImplicLocatie instance);
    partial void InsertGegevensSet(GegevensSet instance);
    partial void UpdateGegevensSet(GegevensSet instance);
    partial void DeleteGegevensSet(GegevensSet instance);
    #endregion
		
		public DataClasses1DataContext() : 
				base(global::DataResource.Properties.Settings.Default.RetPokConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<BronBestand> BronBestands
		{
			get
			{
				return this.GetTable<BronBestand>();
			}
		}
		
		public System.Data.Linq.Table<Project> Projects
		{
			get
			{
				return this.GetTable<Project>();
			}
		}
		
		public System.Data.Linq.Table<GegevensSoort> GegevensSoorts
		{
			get
			{
				return this.GetTable<GegevensSoort>();
			}
		}
		
		public System.Data.Linq.Table<ImplicLocatie> ImplicLocaties
		{
			get
			{
				return this.GetTable<ImplicLocatie>();
			}
		}
		
		public System.Data.Linq.Table<GegevensSet> GegevensSets
		{
			get
			{
				return this.GetTable<GegevensSet>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.BronBestand")]
	public partial class BronBestand : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _GegevensSetId;
		
		private string _BestandsNaam;
		
		private System.DateTime _AanleverDatum;
		
		private EntityRef<GegevensSet> _GegevensSet;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnGegevensSetIdChanging(int value);
    partial void OnGegevensSetIdChanged();
    partial void OnBestandsNaamChanging(string value);
    partial void OnBestandsNaamChanged();
    partial void OnAanleverDatumChanging(System.DateTime value);
    partial void OnAanleverDatumChanged();
    #endregion
		
		public BronBestand()
		{
			this._GegevensSet = default(EntityRef<GegevensSet>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GegevensSetId", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int GegevensSetId
		{
			get
			{
				return this._GegevensSetId;
			}
			set
			{
				if ((this._GegevensSetId != value))
				{
					if (this._GegevensSet.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnGegevensSetIdChanging(value);
					this.SendPropertyChanging();
					this._GegevensSetId = value;
					this.SendPropertyChanged("GegevensSetId");
					this.OnGegevensSetIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BestandsNaam", DbType="VarChar(50) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string BestandsNaam
		{
			get
			{
				return this._BestandsNaam;
			}
			set
			{
				if ((this._BestandsNaam != value))
				{
					this.OnBestandsNaamChanging(value);
					this.SendPropertyChanging();
					this._BestandsNaam = value;
					this.SendPropertyChanged("BestandsNaam");
					this.OnBestandsNaamChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AanleverDatum", DbType="DateTime NOT NULL")]
		public System.DateTime AanleverDatum
		{
			get
			{
				return this._AanleverDatum;
			}
			set
			{
				if ((this._AanleverDatum != value))
				{
					this.OnAanleverDatumChanging(value);
					this.SendPropertyChanging();
					this._AanleverDatum = value;
					this.SendPropertyChanged("AanleverDatum");
					this.OnAanleverDatumChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GegevensSet_BronBestand", Storage="_GegevensSet", ThisKey="GegevensSetId", OtherKey="GegevensSetId", IsForeignKey=true, DeleteOnNull=true, DeleteRule="CASCADE")]
		public GegevensSet GegevensSet
		{
			get
			{
				return this._GegevensSet.Entity;
			}
			set
			{
				GegevensSet previousValue = this._GegevensSet.Entity;
				if (((previousValue != value) 
							|| (this._GegevensSet.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._GegevensSet.Entity = null;
						previousValue.BronBestands.Remove(this);
					}
					this._GegevensSet.Entity = value;
					if ((value != null))
					{
						value.BronBestands.Add(this);
						this._GegevensSetId = value.GegevensSetId;
					}
					else
					{
						this._GegevensSetId = default(int);
					}
					this.SendPropertyChanged("GegevensSet");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Project")]
	public partial class Project : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ProjectId;
		
		private string _ProjectNaam;
		
		private string _ProjectOmschrijving;
		
		private bool _Experimenteel;
		
		private System.Nullable<System.DateTime> _BerekenDatum;
		
		private System.Nullable<int> _ImplicGegevensSetId;
		
		private System.Nullable<int> _FaalkansGegevensSetId;
		
		private System.Nullable<int> _TNOGegevensSetId;
		
		private System.Nullable<int> _ToetspeilGegevensSetId;
		
		private EntityRef<GegevensSet> _GegevensSet;
		
		private EntityRef<GegevensSet> _GegevensSet1;
		
		private EntityRef<GegevensSet> _GegevensSet2;
		
		private EntityRef<GegevensSet> _GegevensSet3;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnProjectIdChanging(int value);
    partial void OnProjectIdChanged();
    partial void OnProjectNaamChanging(string value);
    partial void OnProjectNaamChanged();
    partial void OnProjectOmschrijvingChanging(string value);
    partial void OnProjectOmschrijvingChanged();
    partial void OnExperimenteelChanging(bool value);
    partial void OnExperimenteelChanged();
    partial void OnBerekenDatumChanging(System.Nullable<System.DateTime> value);
    partial void OnBerekenDatumChanged();
    partial void OnImplicGegevensSetIdChanging(System.Nullable<int> value);
    partial void OnImplicGegevensSetIdChanged();
    partial void OnFaalkansGegevensSetIdChanging(System.Nullable<int> value);
    partial void OnFaalkansGegevensSetIdChanged();
    partial void OnTNOGegevensSetIdChanging(System.Nullable<int> value);
    partial void OnTNOGegevensSetIdChanged();
    partial void OnToetspeilGegevensSetIdChanging(System.Nullable<int> value);
    partial void OnToetspeilGegevensSetIdChanged();
    #endregion
		
		public Project()
		{
			this._GegevensSet = default(EntityRef<GegevensSet>);
			this._GegevensSet1 = default(EntityRef<GegevensSet>);
			this._GegevensSet2 = default(EntityRef<GegevensSet>);
			this._GegevensSet3 = default(EntityRef<GegevensSet>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProjectId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ProjectId
		{
			get
			{
				return this._ProjectId;
			}
			set
			{
				if ((this._ProjectId != value))
				{
					this.OnProjectIdChanging(value);
					this.SendPropertyChanging();
					this._ProjectId = value;
					this.SendPropertyChanged("ProjectId");
					this.OnProjectIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProjectNaam", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string ProjectNaam
		{
			get
			{
				return this._ProjectNaam;
			}
			set
			{
				if ((this._ProjectNaam != value))
				{
					this.OnProjectNaamChanging(value);
					this.SendPropertyChanging();
					this._ProjectNaam = value;
					this.SendPropertyChanged("ProjectNaam");
					this.OnProjectNaamChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProjectOmschrijving", DbType="VarChar(256) NOT NULL", CanBeNull=false)]
		public string ProjectOmschrijving
		{
			get
			{
				return this._ProjectOmschrijving;
			}
			set
			{
				if ((this._ProjectOmschrijving != value))
				{
					this.OnProjectOmschrijvingChanging(value);
					this.SendPropertyChanging();
					this._ProjectOmschrijving = value;
					this.SendPropertyChanged("ProjectOmschrijving");
					this.OnProjectOmschrijvingChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Experimenteel", DbType="Bit NOT NULL")]
		public bool Experimenteel
		{
			get
			{
				return this._Experimenteel;
			}
			set
			{
				if ((this._Experimenteel != value))
				{
					this.OnExperimenteelChanging(value);
					this.SendPropertyChanging();
					this._Experimenteel = value;
					this.SendPropertyChanged("Experimenteel");
					this.OnExperimenteelChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BerekenDatum", DbType="DateTime")]
		public System.Nullable<System.DateTime> BerekenDatum
		{
			get
			{
				return this._BerekenDatum;
			}
			set
			{
				if ((this._BerekenDatum != value))
				{
					this.OnBerekenDatumChanging(value);
					this.SendPropertyChanging();
					this._BerekenDatum = value;
					this.SendPropertyChanged("BerekenDatum");
					this.OnBerekenDatumChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ImplicGegevensSetId", DbType="Int")]
		public System.Nullable<int> ImplicGegevensSetId
		{
			get
			{
				return this._ImplicGegevensSetId;
			}
			set
			{
				if ((this._ImplicGegevensSetId != value))
				{
					if (this._GegevensSet.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnImplicGegevensSetIdChanging(value);
					this.SendPropertyChanging();
					this._ImplicGegevensSetId = value;
					this.SendPropertyChanged("ImplicGegevensSetId");
					this.OnImplicGegevensSetIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FaalkansGegevensSetId", DbType="Int")]
		public System.Nullable<int> FaalkansGegevensSetId
		{
			get
			{
				return this._FaalkansGegevensSetId;
			}
			set
			{
				if ((this._FaalkansGegevensSetId != value))
				{
					if (this._GegevensSet1.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnFaalkansGegevensSetIdChanging(value);
					this.SendPropertyChanging();
					this._FaalkansGegevensSetId = value;
					this.SendPropertyChanged("FaalkansGegevensSetId");
					this.OnFaalkansGegevensSetIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TNOGegevensSetId", DbType="Int")]
		public System.Nullable<int> TNOGegevensSetId
		{
			get
			{
				return this._TNOGegevensSetId;
			}
			set
			{
				if ((this._TNOGegevensSetId != value))
				{
					if (this._GegevensSet2.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnTNOGegevensSetIdChanging(value);
					this.SendPropertyChanging();
					this._TNOGegevensSetId = value;
					this.SendPropertyChanged("TNOGegevensSetId");
					this.OnTNOGegevensSetIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ToetspeilGegevensSetId", DbType="Int")]
		public System.Nullable<int> ToetspeilGegevensSetId
		{
			get
			{
				return this._ToetspeilGegevensSetId;
			}
			set
			{
				if ((this._ToetspeilGegevensSetId != value))
				{
					if (this._GegevensSet3.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnToetspeilGegevensSetIdChanging(value);
					this.SendPropertyChanging();
					this._ToetspeilGegevensSetId = value;
					this.SendPropertyChanged("ToetspeilGegevensSetId");
					this.OnToetspeilGegevensSetIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GegevensSet_Project", Storage="_GegevensSet", ThisKey="ImplicGegevensSetId", OtherKey="GegevensSetId", IsForeignKey=true)]
		public GegevensSet GegevensSet
		{
			get
			{
				return this._GegevensSet.Entity;
			}
			set
			{
				GegevensSet previousValue = this._GegevensSet.Entity;
				if (((previousValue != value) 
							|| (this._GegevensSet.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._GegevensSet.Entity = null;
						previousValue.Projects.Remove(this);
					}
					this._GegevensSet.Entity = value;
					if ((value != null))
					{
						value.Projects.Add(this);
						this._ImplicGegevensSetId = value.GegevensSetId;
					}
					else
					{
						this._ImplicGegevensSetId = default(Nullable<int>);
					}
					this.SendPropertyChanged("GegevensSet");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GegevensSet_Project1", Storage="_GegevensSet1", ThisKey="FaalkansGegevensSetId", OtherKey="GegevensSetId", IsForeignKey=true)]
		public GegevensSet GegevensSet1
		{
			get
			{
				return this._GegevensSet1.Entity;
			}
			set
			{
				GegevensSet previousValue = this._GegevensSet1.Entity;
				if (((previousValue != value) 
							|| (this._GegevensSet1.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._GegevensSet1.Entity = null;
						previousValue.Projects1.Remove(this);
					}
					this._GegevensSet1.Entity = value;
					if ((value != null))
					{
						value.Projects1.Add(this);
						this._FaalkansGegevensSetId = value.GegevensSetId;
					}
					else
					{
						this._FaalkansGegevensSetId = default(Nullable<int>);
					}
					this.SendPropertyChanged("GegevensSet1");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GegevensSet_Project2", Storage="_GegevensSet2", ThisKey="TNOGegevensSetId", OtherKey="GegevensSetId", IsForeignKey=true)]
		public GegevensSet GegevensSet2
		{
			get
			{
				return this._GegevensSet2.Entity;
			}
			set
			{
				GegevensSet previousValue = this._GegevensSet2.Entity;
				if (((previousValue != value) 
							|| (this._GegevensSet2.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._GegevensSet2.Entity = null;
						previousValue.Projects2.Remove(this);
					}
					this._GegevensSet2.Entity = value;
					if ((value != null))
					{
						value.Projects2.Add(this);
						this._TNOGegevensSetId = value.GegevensSetId;
					}
					else
					{
						this._TNOGegevensSetId = default(Nullable<int>);
					}
					this.SendPropertyChanged("GegevensSet2");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GegevensSet_Project3", Storage="_GegevensSet3", ThisKey="ToetspeilGegevensSetId", OtherKey="GegevensSetId", IsForeignKey=true)]
		public GegevensSet GegevensSet3
		{
			get
			{
				return this._GegevensSet3.Entity;
			}
			set
			{
				GegevensSet previousValue = this._GegevensSet3.Entity;
				if (((previousValue != value) 
							|| (this._GegevensSet3.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._GegevensSet3.Entity = null;
						previousValue.Projects3.Remove(this);
					}
					this._GegevensSet3.Entity = value;
					if ((value != null))
					{
						value.Projects3.Add(this);
						this._ToetspeilGegevensSetId = value.GegevensSetId;
					}
					else
					{
						this._ToetspeilGegevensSetId = default(Nullable<int>);
					}
					this.SendPropertyChanged("GegevensSet3");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.GegevensSoort")]
	public partial class GegevensSoort : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _GegevensSoortId;
		
		private string _GegevensSoortNaam;
		
		private EntitySet<GegevensSet> _GegevensSets;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnGegevensSoortIdChanging(int value);
    partial void OnGegevensSoortIdChanged();
    partial void OnGegevensSoortNaamChanging(string value);
    partial void OnGegevensSoortNaamChanged();
    #endregion
		
		public GegevensSoort()
		{
			this._GegevensSets = new EntitySet<GegevensSet>(new Action<GegevensSet>(this.attach_GegevensSets), new Action<GegevensSet>(this.detach_GegevensSets));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GegevensSoortId", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int GegevensSoortId
		{
			get
			{
				return this._GegevensSoortId;
			}
			set
			{
				if ((this._GegevensSoortId != value))
				{
					this.OnGegevensSoortIdChanging(value);
					this.SendPropertyChanging();
					this._GegevensSoortId = value;
					this.SendPropertyChanged("GegevensSoortId");
					this.OnGegevensSoortIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GegevensSoortNaam", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string GegevensSoortNaam
		{
			get
			{
				return this._GegevensSoortNaam;
			}
			set
			{
				if ((this._GegevensSoortNaam != value))
				{
					this.OnGegevensSoortNaamChanging(value);
					this.SendPropertyChanging();
					this._GegevensSoortNaam = value;
					this.SendPropertyChanged("GegevensSoortNaam");
					this.OnGegevensSoortNaamChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GegevensSoort_GegevensSet", Storage="_GegevensSets", ThisKey="GegevensSoortId", OtherKey="GegevensSoortId")]
		public EntitySet<GegevensSet> GegevensSets
		{
			get
			{
				return this._GegevensSets;
			}
			set
			{
				this._GegevensSets.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_GegevensSets(GegevensSet entity)
		{
			this.SendPropertyChanging();
			entity.GegevensSoort = this;
		}
		
		private void detach_GegevensSets(GegevensSet entity)
		{
			this.SendPropertyChanging();
			entity.GegevensSoort = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ImplicLocatie")]
	public partial class ImplicLocatie : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ImplicLocatieId;
		
		private int _GegevensSetId;
		
		private string _LocatieNaam;
		
		private short _Volgnummer;
		
		private EntityRef<GegevensSet> _GegevensSet;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnImplicLocatieIdChanging(int value);
    partial void OnImplicLocatieIdChanged();
    partial void OnGegevensSetIdChanging(int value);
    partial void OnGegevensSetIdChanged();
    partial void OnLocatieNaamChanging(string value);
    partial void OnLocatieNaamChanged();
    partial void OnVolgnummerChanging(short value);
    partial void OnVolgnummerChanged();
    #endregion
		
		public ImplicLocatie()
		{
			this._GegevensSet = default(EntityRef<GegevensSet>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ImplicLocatieId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ImplicLocatieId
		{
			get
			{
				return this._ImplicLocatieId;
			}
			set
			{
				if ((this._ImplicLocatieId != value))
				{
					this.OnImplicLocatieIdChanging(value);
					this.SendPropertyChanging();
					this._ImplicLocatieId = value;
					this.SendPropertyChanged("ImplicLocatieId");
					this.OnImplicLocatieIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GegevensSetId", DbType="Int NOT NULL")]
		public int GegevensSetId
		{
			get
			{
				return this._GegevensSetId;
			}
			set
			{
				if ((this._GegevensSetId != value))
				{
					if (this._GegevensSet.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnGegevensSetIdChanging(value);
					this.SendPropertyChanging();
					this._GegevensSetId = value;
					this.SendPropertyChanged("GegevensSetId");
					this.OnGegevensSetIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LocatieNaam", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string LocatieNaam
		{
			get
			{
				return this._LocatieNaam;
			}
			set
			{
				if ((this._LocatieNaam != value))
				{
					this.OnLocatieNaamChanging(value);
					this.SendPropertyChanging();
					this._LocatieNaam = value;
					this.SendPropertyChanged("LocatieNaam");
					this.OnLocatieNaamChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Volgnummer", DbType="SmallInt NOT NULL")]
		public short Volgnummer
		{
			get
			{
				return this._Volgnummer;
			}
			set
			{
				if ((this._Volgnummer != value))
				{
					this.OnVolgnummerChanging(value);
					this.SendPropertyChanging();
					this._Volgnummer = value;
					this.SendPropertyChanged("Volgnummer");
					this.OnVolgnummerChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GegevensSet_ImplicLocatie", Storage="_GegevensSet", ThisKey="GegevensSetId", OtherKey="GegevensSetId", IsForeignKey=true, DeleteOnNull=true, DeleteRule="CASCADE")]
		public GegevensSet GegevensSet
		{
			get
			{
				return this._GegevensSet.Entity;
			}
			set
			{
				GegevensSet previousValue = this._GegevensSet.Entity;
				if (((previousValue != value) 
							|| (this._GegevensSet.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._GegevensSet.Entity = null;
						previousValue.ImplicLocaties.Remove(this);
					}
					this._GegevensSet.Entity = value;
					if ((value != null))
					{
						value.ImplicLocaties.Add(this);
						this._GegevensSetId = value.GegevensSetId;
					}
					else
					{
						this._GegevensSetId = default(int);
					}
					this.SendPropertyChanged("GegevensSet");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.GegevensSet")]
	public partial class GegevensSet : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _GegevensSetId;
		
		private int _GegevensSoortId;
		
		private string _GegevensSetNaam;
		
		private bool _SafAanwezig;
		
		private bool _SofAanwezig;
		
		private bool _Beschikbaar;
		
		private EntitySet<BronBestand> _BronBestands;
		
		private EntitySet<Project> _Projects;
		
		private EntitySet<Project> _Projects1;
		
		private EntitySet<Project> _Projects2;
		
		private EntitySet<Project> _Projects3;
		
		private EntitySet<ImplicLocatie> _ImplicLocaties;
		
		private EntityRef<GegevensSoort> _GegevensSoort;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnGegevensSetIdChanging(int value);
    partial void OnGegevensSetIdChanged();
    partial void OnGegevensSoortIdChanging(int value);
    partial void OnGegevensSoortIdChanged();
    partial void OnGegevensSetNaamChanging(string value);
    partial void OnGegevensSetNaamChanged();
    partial void OnSafAanwezigChanging(bool value);
    partial void OnSafAanwezigChanged();
    partial void OnSofAanwezigChanging(bool value);
    partial void OnSofAanwezigChanged();
    partial void OnBeschikbaarChanging(bool value);
    partial void OnBeschikbaarChanged();
    #endregion
		
		public GegevensSet()
		{
			this._BronBestands = new EntitySet<BronBestand>(new Action<BronBestand>(this.attach_BronBestands), new Action<BronBestand>(this.detach_BronBestands));
			this._Projects = new EntitySet<Project>(new Action<Project>(this.attach_Projects), new Action<Project>(this.detach_Projects));
			this._Projects1 = new EntitySet<Project>(new Action<Project>(this.attach_Projects1), new Action<Project>(this.detach_Projects1));
			this._Projects2 = new EntitySet<Project>(new Action<Project>(this.attach_Projects2), new Action<Project>(this.detach_Projects2));
			this._Projects3 = new EntitySet<Project>(new Action<Project>(this.attach_Projects3), new Action<Project>(this.detach_Projects3));
			this._ImplicLocaties = new EntitySet<ImplicLocatie>(new Action<ImplicLocatie>(this.attach_ImplicLocaties), new Action<ImplicLocatie>(this.detach_ImplicLocaties));
			this._GegevensSoort = default(EntityRef<GegevensSoort>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GegevensSetId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int GegevensSetId
		{
			get
			{
				return this._GegevensSetId;
			}
			set
			{
				if ((this._GegevensSetId != value))
				{
					this.OnGegevensSetIdChanging(value);
					this.SendPropertyChanging();
					this._GegevensSetId = value;
					this.SendPropertyChanged("GegevensSetId");
					this.OnGegevensSetIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GegevensSoortId", DbType="Int NOT NULL")]
		public int GegevensSoortId
		{
			get
			{
				return this._GegevensSoortId;
			}
			set
			{
				if ((this._GegevensSoortId != value))
				{
					if (this._GegevensSoort.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnGegevensSoortIdChanging(value);
					this.SendPropertyChanging();
					this._GegevensSoortId = value;
					this.SendPropertyChanged("GegevensSoortId");
					this.OnGegevensSoortIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GegevensSetNaam", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string GegevensSetNaam
		{
			get
			{
				return this._GegevensSetNaam;
			}
			set
			{
				if ((this._GegevensSetNaam != value))
				{
					this.OnGegevensSetNaamChanging(value);
					this.SendPropertyChanging();
					this._GegevensSetNaam = value;
					this.SendPropertyChanged("GegevensSetNaam");
					this.OnGegevensSetNaamChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SafAanwezig", DbType="Bit NOT NULL")]
		public bool SafAanwezig
		{
			get
			{
				return this._SafAanwezig;
			}
			set
			{
				if ((this._SafAanwezig != value))
				{
					this.OnSafAanwezigChanging(value);
					this.SendPropertyChanging();
					this._SafAanwezig = value;
					this.SendPropertyChanged("SafAanwezig");
					this.OnSafAanwezigChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SofAanwezig", DbType="Bit NOT NULL")]
		public bool SofAanwezig
		{
			get
			{
				return this._SofAanwezig;
			}
			set
			{
				if ((this._SofAanwezig != value))
				{
					this.OnSofAanwezigChanging(value);
					this.SendPropertyChanging();
					this._SofAanwezig = value;
					this.SendPropertyChanged("SofAanwezig");
					this.OnSofAanwezigChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Beschikbaar", DbType="Bit NOT NULL")]
		public bool Beschikbaar
		{
			get
			{
				return this._Beschikbaar;
			}
			set
			{
				if ((this._Beschikbaar != value))
				{
					this.OnBeschikbaarChanging(value);
					this.SendPropertyChanging();
					this._Beschikbaar = value;
					this.SendPropertyChanged("Beschikbaar");
					this.OnBeschikbaarChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GegevensSet_BronBestand", Storage="_BronBestands", ThisKey="GegevensSetId", OtherKey="GegevensSetId")]
		public EntitySet<BronBestand> BronBestands
		{
			get
			{
				return this._BronBestands;
			}
			set
			{
				this._BronBestands.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GegevensSet_Project", Storage="_Projects", ThisKey="GegevensSetId", OtherKey="ImplicGegevensSetId")]
		public EntitySet<Project> Projects
		{
			get
			{
				return this._Projects;
			}
			set
			{
				this._Projects.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GegevensSet_Project1", Storage="_Projects1", ThisKey="GegevensSetId", OtherKey="FaalkansGegevensSetId")]
		public EntitySet<Project> Projects1
		{
			get
			{
				return this._Projects1;
			}
			set
			{
				this._Projects1.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GegevensSet_Project2", Storage="_Projects2", ThisKey="GegevensSetId", OtherKey="TNOGegevensSetId")]
		public EntitySet<Project> Projects2
		{
			get
			{
				return this._Projects2;
			}
			set
			{
				this._Projects2.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GegevensSet_Project3", Storage="_Projects3", ThisKey="GegevensSetId", OtherKey="ToetspeilGegevensSetId")]
		public EntitySet<Project> Projects3
		{
			get
			{
				return this._Projects3;
			}
			set
			{
				this._Projects3.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GegevensSet_ImplicLocatie", Storage="_ImplicLocaties", ThisKey="GegevensSetId", OtherKey="GegevensSetId")]
		public EntitySet<ImplicLocatie> ImplicLocaties
		{
			get
			{
				return this._ImplicLocaties;
			}
			set
			{
				this._ImplicLocaties.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GegevensSoort_GegevensSet", Storage="_GegevensSoort", ThisKey="GegevensSoortId", OtherKey="GegevensSoortId", IsForeignKey=true)]
		public GegevensSoort GegevensSoort
		{
			get
			{
				return this._GegevensSoort.Entity;
			}
			set
			{
				GegevensSoort previousValue = this._GegevensSoort.Entity;
				if (((previousValue != value) 
							|| (this._GegevensSoort.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._GegevensSoort.Entity = null;
						previousValue.GegevensSets.Remove(this);
					}
					this._GegevensSoort.Entity = value;
					if ((value != null))
					{
						value.GegevensSets.Add(this);
						this._GegevensSoortId = value.GegevensSoortId;
					}
					else
					{
						this._GegevensSoortId = default(int);
					}
					this.SendPropertyChanged("GegevensSoort");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_BronBestands(BronBestand entity)
		{
			this.SendPropertyChanging();
			entity.GegevensSet = this;
		}
		
		private void detach_BronBestands(BronBestand entity)
		{
			this.SendPropertyChanging();
			entity.GegevensSet = null;
		}
		
		private void attach_Projects(Project entity)
		{
			this.SendPropertyChanging();
			entity.GegevensSet = this;
		}
		
		private void detach_Projects(Project entity)
		{
			this.SendPropertyChanging();
			entity.GegevensSet = null;
		}
		
		private void attach_Projects1(Project entity)
		{
			this.SendPropertyChanging();
			entity.GegevensSet1 = this;
		}
		
		private void detach_Projects1(Project entity)
		{
			this.SendPropertyChanging();
			entity.GegevensSet1 = null;
		}
		
		private void attach_Projects2(Project entity)
		{
			this.SendPropertyChanging();
			entity.GegevensSet2 = this;
		}
		
		private void detach_Projects2(Project entity)
		{
			this.SendPropertyChanging();
			entity.GegevensSet2 = null;
		}
		
		private void attach_Projects3(Project entity)
		{
			this.SendPropertyChanging();
			entity.GegevensSet3 = this;
		}
		
		private void detach_Projects3(Project entity)
		{
			this.SendPropertyChanging();
			entity.GegevensSet3 = null;
		}
		
		private void attach_ImplicLocaties(ImplicLocatie entity)
		{
			this.SendPropertyChanging();
			entity.GegevensSet = this;
		}
		
		private void detach_ImplicLocaties(ImplicLocatie entity)
		{
			this.SendPropertyChanging();
			entity.GegevensSet = null;
		}
	}
}
#pragma warning restore 1591

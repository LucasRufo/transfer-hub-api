{{/*
Expand the name of the chart.
*/}}
{{- define "transfer-hub-api.name" -}}
{{- .Chart.Name | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "transfer-hub-api.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "transfer-hub-api.labels" -}}
helm.sh/chart: {{ include "transfer-hub-api.chart" . }}
{{ include "transfer-hub-api.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "transfer-hub-api.selectorLabels" -}}
app.kubernetes.io/name: {{ include "transfer-hub-api.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "transfer-hub-api.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "transfer-hub-api.name" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}

<template>
  <v-layout column>
    <v-flex xs12>
      <v-layout justify-center row fill-height>
        <editor v-model="rawJson" @init="editorInit" @input="onInput" lang="json" :theme="`${nightmode ? 'twilight' : 'dawn'}`" width="100%" height="100%" class="editor"></editor>
        <editor v-model="formattedJson" @init="editorInit" lang="json" :theme="`${nightmode ? 'twilight' : 'dawn'}`" width="100%" height="100%" class="editor"></editor>
      </v-layout>
    </v-flex>
    <v-snackbar
      v-model="snackbar.open"
      :color="snackbar.color"
      :timeout="snackbar.timeout"
      :top="snackbar.position.y === 'top'"
      :bottom="snackbar.position.y === 'bottom'"
      :left="snackbar.position.x === 'left'"
      :right="snackbar.position.x === 'right'"
      :multi-line="snackbar.multiline"
    >
      {{ snackbar.text }}
      <v-btn
        :dark="nightmode"
        flat
        @click="snackbar.open = false">
        Close
      </v-btn>
    </v-snackbar>
  </v-layout>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import axios from 'axios';

@Component({
  props: ['nightmode', 'settings', 'apiUrl'],
  components: {
    editor: require('vue2-ace-editor'),
  },
  data() {
    return {
      rawJson: '',
      formattedJson: '',
      compact: false,
      snackbar: {
        open: false,
        color: '',
        position: {
          x: 'center',
          y: 'bottom',
        },
        timeout: 6000,
        text: '',
        multiline: false,
      },
    };
  },
  methods: {
    editorInit(editor: any) {
      require('brace/ext/language_tools');
      require('brace/mode/json');
      require('brace/theme/dawn');
      require('brace/theme/twilight');
      editor.session.setUseWrapMode(true);
    },
    onInput(delta: string) {
      if (!delta || delta.length === 0) {
        return;
      }

      const getQueryString = (settings: any) => {
        let qs = `?indent=${settings.tabWidth.width}`;
        if (settings.useTabs) {
          qs += `&indentChar=${encodeURI('\t')}`;
        }
        if (settings.minify) {
          qs += `&compact=${true}`;
        }
        qs += `&quoteChar=${settings.quoteChars.option}`;
        if (!settings.quoteNames) {
          qs += `&quoteNames=${false}`;
        }
        return qs;
      };

      axios.post(`${this.$props.apiUrl}/json${getQueryString(this.$props.settings)}`, delta, {
        headers: {'Access-Control-Allow-Origin': '*', 'Content-Type': 'application/json'},
        responseType: 'application/json',
        transformResponse: undefined,
      })
      .then((response: any) => {
        this.$data.formattedJson = JSON.parse(response.data);
        this.$data.snackbar.text = 'JSON is valid!';
        this.$data.snackbar.color = 'success';
        this.$data.snackbar.multiline = false;
        this.$data.snackbar.open = true;
      })
      .catch((error: any) => {
        this.$data.snackbar.text = `${error.response.data}`;
        this.$data.snackbar.color = 'error';
        this.$data.snackbar.multiline = true;
        this.$data.snackbar.open = true;
      });
    },
  },
})
export default class Editors extends Vue {
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>

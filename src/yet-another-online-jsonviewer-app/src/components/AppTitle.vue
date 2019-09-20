<template>
    <span class="title font-weight-light">Yet Another <strong class="light-green--text text--lighten-1">{JSON formatter}</strong><span v-if="version" class="caption version grey--text"> v{{version}}</span></span>
</template>

<script lang="ts">
import {Component, Vue} from 'vue-property-decorator';

@Component({
    props: ['version'],
})
export default class AppTitle extends Vue {
}
</script>

<style scoped>
</style>